using Manufaktura.Controls.Audio;
using Manufaktura.Controls.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveDots
{
    /*
 * Copyright 2018 Manufaktura Programów Jacek Salamon http://musicengravingcontrols.com/
 * MIT LICENCE
 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
    /// <summary>
    /// ScorePlayer that uses Task API for scheduling playback of elements on a playback timeline
    /// </summary>
    public abstract class MyTaskScorePlayer : ScorePlayer
    {
        private IEnumerator<TimelineElement<IHasDuration>> timelineIterator;

        /// <summary>
        /// Initializes a new instance of TaskScorePlayer
        /// </summary>
        /// <param name="score"></param>
        public MyTaskScorePlayer(Score score) : base(score)
        {
            timelineIterator = EnumerateTimeline().GetEnumerator();
            WasPaused = false;
        }

        /// <summary>
        /// Currently played element
        /// </summary>
        public override MusicalSymbol CurrentElement
        {
            get
            {
                return EnumerateTimeline().FirstOrDefault(t => t.When == ElapsedTime)?.What as MusicalSymbol;
            }

            protected set
            {
                throw new Exception("CurrentElement method is readonly on this class.");
            }
        }
        public bool WasPaused;

        /// <summary>
        /// Elapsed time
        /// </summary>
        public override TimeSpan ElapsedTime
        {
            get
            {
                return base.ElapsedTime;
            }

            set
            {
                base.ElapsedTime = value;
                OnPropertyChanged(nameof(CurrentElement));
                OnPropertyChanged(nameof(CurrentPosition));
            }
        }

        /// <summary>
        /// Pauses playback
        /// </summary>
        public override void Pause()
        {
            State = PlaybackState.Paused;
            WasPaused = true;
        }

        /// <summary>
        /// Starts playback
        /// </summary>
        public override async void Play()
        {
            State = PlaybackState.Playing;
            await Task.Factory.StartNew(PlayInternal);

            //State = PlaybackState.Idle;
        }

        /// <summary>
        /// Stops playback
        /// </summary>
        public override void Stop()
        {
            State = PlaybackState.Idle;
            timelineIterator = EnumerateTimeline().GetEnumerator();
        }

        /// <summary>
        /// Plays a queue of TimelineElements
        /// </summary>
        /// <param name="simultaneousElements"></param>
        protected virtual void PlayQueue(Queue<TimelineElement<IHasDuration>> simultaneousElements)
        {
            lock (simultaneousElements)
            {
                while (simultaneousElements.Any())
                {
                    var element = simultaneousElements.Dequeue();
                    if (ElapsedTime != element.When) ElapsedTime = element.When;
                    var note = element.What as Note;
                    if (note == null) continue;
                    PlayElement(note);
                }
            }
        }

        private async void PlayInternal()
        {
            var simultaneousElements = new Queue<TimelineElement<IHasDuration>>();

            TimelineElement<IHasDuration> previousElement = null;
            TimeSpan lastAwaitedDuration = TimeSpan.Zero;

            /// No sé si esto va a tener problemas con los acordes
            if (WasPaused && State == PlaybackState.Playing)
            {
                var timelineElement = timelineIterator.Current;
                if (previousElement != null && timelineElement.When > previousElement.When)
                {
                    await Task.Delay(lastAwaitedDuration == TimeSpan.Zero ? previousElement.When : previousElement.When - lastAwaitedDuration);
                    PlayQueue(simultaneousElements);
                    lastAwaitedDuration = previousElement.When;
                }

                simultaneousElements.Enqueue(timelineElement);
                previousElement = timelineElement;
                WasPaused = false;
            }
            ///Hasta aquí trozo problematico, si da problemas quitar. Sirve para no perder notas al pausar y volver a reproducir
            while (timelineIterator.MoveNext()) //intrakeable el error --> System.InvalidOperationException: 'La secuencia no contiene elementos', ocurre al pasar de notas con las flechas

            {
                var timelineElement = timelineIterator.Current;
                try
                {
                    if (State != PlaybackState.Playing) break;

                    if (previousElement != null && timelineElement.When > previousElement.When)
                    {
                        await Task.Delay(lastAwaitedDuration == TimeSpan.Zero ? previousElement.When : previousElement.When - lastAwaitedDuration);
                        PlayQueue(simultaneousElements);
                        lastAwaitedDuration = previousElement.When;
                    }

                    simultaneousElements.Enqueue(timelineElement);
                    previousElement = timelineElement;
                }
                catch (Exception e)
                {
                    
                }
            }

            if (simultaneousElements.Any())
            {
                await Task.Delay(lastAwaitedDuration == TimeSpan.Zero ? previousElement.When : previousElement.When - lastAwaitedDuration);
                PlayQueue(simultaneousElements);
            }
            if (State != PlaybackState.Paused)
            {
                State = PlaybackState.Idle;
                timelineIterator = EnumerateTimeline().GetEnumerator();
            }

        }
    }
}

