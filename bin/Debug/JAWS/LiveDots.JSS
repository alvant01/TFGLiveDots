include "hjconst.jsh"

Const
	COM_OBJ="LiveDots.LiveDotsCOMObj"


Globals
	Object COMObj,
	string error_code,
	Handle obj

;-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
; Evento de inicio que se ejecuta cada vez que la aplicaci�n obtiene el foco
;-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

void Function AutoStartEvent ()	
	SetJCFOption(OPT_TYPING_ECHO,1)		
	Let COMObj = GetObject (COM_OBJ)	
	;error_code = COMObj.getErrorCode()	
	;SayMessage(ot_message,error_code)
EndFunction


;-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
; Evento para verbalizar el caracter actual
;-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
void Function SayCharacter ()
	SpeechOn()
	var  String char,
	Handle applWnd,
	string txt	
	Pause()
	If (!COMObj) Then	
		 COMObj = GetObject (COM_OBJ)
	endif
	If (COMObj && COMObj.HasObject()) then
			let txt = COMObj.GetElement()	    
		    Pause()
			if(txt) then		
				SayMessage(OT_MESSAGE, txt )			
			Else
				SayCharacter ()			
			endif
	Else
		SayCharacter ()		
	EndIf
EndFunction


void Function FocusChangedEventEx(handle a, int b, int c, handle d, int e, int f, int g)
	SpeechOn()
	var  String char,
	Handle applWnd
	Pause()
	If (!COMObj) Then	
		 COMObj = GetObject (COM_OBJ)
	endif
	If (COMObj && COMObj.IsTextBoxFocused()) then
		Pause()
		SayCharacter()
		SpeechOff()
	Else
		FocusChangedEventEx(a,b,c,d,e,f,g)
	EndIf
EndFunction
