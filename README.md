# OH Midi Player

OH Midi Player is a MIDI player that simulates keyboard inputs based on MIDI events into the video game Once Human, providing a visual representation of the MIDI playback through a virtual piano keyboard.

## Features

- **MIDI Playback**: Simulate keyboard inputs based on MIDI notes.
- **Visual Keyboard**: Highlights virtual keys in a piano layout corresponding to the MIDI notes being played.
- **Settings Persistence**: Saves user preferences and settings for future sessions.

## Usage

### MIDI Playback

1. **Load a MIDI file**: 
   - Click `Select Library` to browse and select a folder containing MIDI files.
   - MIDI files will be listed in the `Library` panel.
2. **Start playback**: 
   - Select a MIDI file and press `F5` to start playback.
   - Virtual piano keys will highlight according to the notes being played.
3. **Stop playback**: 
   - Press `F6` to stop the playback.

### Hotkeys

- **F5**: Start playback.
- **F6**: Stop playback.
- **Shift + F4**: Open the debug form for additional diagnostics.


## Issues

If you encounter any bugs or have suggestions, please [open an issue](https://github.com/your-username/OH Midi Player/issues) on GitHub.

## License

This project is licensed under the MIT License.

## Acknowledgements

- [NAudio](https://github.com/naudio/NAudio) for MIDI handling.
- [WindowsInput](https://inputsimulator.codeplex.com/) for simulating keyboard input.
- [Psytech's Once Human Midi Maestro](https://github.com/Psystec/Once-Human-Midi-Maestro) for providing the ground work for this program and assisting in my conversion to C#. Couldn't have done it without him!
