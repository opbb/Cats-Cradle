# Cats-Cradle
Our method of sharing files for this game jam Oct 2022.

Yay READMEs are always nice.

---

To implement the FMOD to Unity Package and Audio Assets:

Download the latest version of FMOD for Unity in the Unity Asset store or on the FMOD Download site

1) Assets -> Import Package -> fmodstudio20207.unitypackage
2) Import All 
3) Welcome -> Start
   Updating -> Make Sure Both Boxes are Checked -> Next
   Linking -> Multiple Platform Build -> Select the fmodAssets folder
   Listener -> Replace Unity Listener with FMOD Audio Listener
   Unity Audio -> Disable built in audio
   Unity Sources -> Next
   Source Control -> Close



Make sure to replace the AudioListener component (I think its in the Main Camera) with the FMOD Studio Listener Component. Sound won't register if it's not FMOD's listener


To test, just add an empty game object in the SampleScene and add the FMOD Event Emitter Component
Under Play Event -> Object Enable
Under Event -> Search -> Events -> Misc -> Misc_Progression
Play the game and activate and reactivate the GameObject and you should be able to hear the glorious sound of success

--






