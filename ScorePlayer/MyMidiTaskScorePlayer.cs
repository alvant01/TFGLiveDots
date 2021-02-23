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

using Manufaktura.Controls.Desktop.Audio.Midi;
using Manufaktura.Controls.Model;
using Manufaktura.Music.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveDots
{
    public class MyMidiTaskScorePlayer : MyChannelSelectingTaskScorePlayer, IDisposable
    {
        private static Lazy<IEnumerable<MidiDevice>> availableDevices = new Lazy<IEnumerable<MidiDevice>>(() =>
        new ReadOnlyCollection<MidiDevice>(Enumerable.Range(0, MidiDevice.DeviceCount).Select(i => new MidiDevice(i,
            Encoding.ASCII.GetString(MidiDevice.GetDeviceCapabilities(i).name))).ToList()));

        private MidiDevice outDevice;

        public MyMidiTaskScorePlayer(Score score) : this(score, new MidiDevice(0, "default"))
        {
        }

        public MyMidiTaskScorePlayer(Score score, MidiDevice device) : base(score)
        {
            outDevice = device;
            if (!outDevice.IsOpen) outDevice.Open();
        }

        public static IEnumerable<MidiDevice> AvailableDevices => availableDevices.Value;

        public void Dispose()
        {
            for (int i = 0; i < Score.Staves.Count * 2; i++)
            {
                ChannelMessageBuilder builder = new ChannelMessageBuilder();
                builder.Command = ChannelCommand.NoteOff;
                builder.Data2 = i;
                builder.Build();

                outDevice.Send(builder.Result);
            }
            outDevice.Dispose();
        }

        public override async void PlayElement(MusicalSymbol element)
        {
            // hay un problema con las staff, al crear una nota, nunca se asgina un staff, lo cual es un problemon
            var note = element as Note;            
            if (note == null /*|| note.Staff == null*/) return;

            if (note.TieType == NoteTieType.Stop || note.TieType == NoteTieType.StopAndStartAnother) return;
            var firstNoteInMeasure = element.Measure?.Elements.IndexOf(note) == 0;

            //var channelNumber = GetChannelNumber(Score.Staves.IndexOf(note.Staff)); // no entiendo que busca en el staff
            var channelNumber = 1; //test
            //var actualChannelNumber = (pitchesPlaying[channelNumber].Contains(note.MidiPitch)) ? channelNumber + 1 : channelNumber;
            var actualChannelNumber = 1; //test

            if (!pitchesPlaying[channelNumber].Contains(note.MidiPitch)) 
                pitchesPlaying[channelNumber].Add(note.MidiPitch);
            outDevice.Send(note, true, actualChannelNumber, firstNoteInMeasure ? 127 : 100);

            //este await es para parar la nota al pasarse del rhytmic duration
            await Task.Delay(new RhythmicDuration(note.BaseDuration.DenominatorAsPowerOfTwo, note.NumberOfDots).ToTimeSpan(Tempo));

            outDevice.Send(note, false, actualChannelNumber);
            if (pitchesPlaying[channelNumber].Contains(note.MidiPitch)) pitchesPlaying[channelNumber].Remove(note.MidiPitch);
        }

        public void SetInstrument(GeneralMidiInstrument instrument)
        {
            foreach (var staff in Score.Staves) SetInstrument(staff, instrument);
        }

        public void SetInstrument(Staff staff, GeneralMidiInstrument instrument)
        {
            if (!Score.Staves.Contains(staff))
                throw new Exception($"Staff {staff} is not a part of the score associated with this player.");

            var channel = GetChannelNumber(Score.Staves.IndexOf(staff));
            for (var i = channel; i <= channel + 1; i++)
            {
                var builder = new ChannelMessageBuilder
                {
                    MidiChannel = i,
                    Data1 = (int)instrument,
                    Command = ChannelCommand.ProgramChange
                };
                builder.Build();
                outDevice.Send(builder.Result);
            }
        }
    }
}
