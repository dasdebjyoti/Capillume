# Capillume

Capillume is a lightweight Windows screenshot automation tool. Capture your entire desktop or the active window on a schedule, save images in your preferred format, and keep the application available from the system tray.

## Features

- Capture all connected displays or the active window.
- Capture a screenshot immediately when scheduled capture starts.
- Configure the capture interval in minutes.
- Save screenshots to a custom folder.
- Save as PNG, JPG, BMP, or WebP.
- Configure JPG and WebP image quality.
- Show desktop notifications after screenshots are saved.
- Start automatically with Windows.
- Use the **Capture Now** command from the system tray.
- Minimize to the system tray instead of closing the application.

## Requirements

- Windows 10 or later.
- .NET 10 SDK/runtime with Windows desktop support.

## Build and run

Clone the repository and open `Capillume.slnx` in Visual Studio, or run the following commands from the repository directory:

```powershell
dotnet restore
dotnet build
dotnet run --project .\Capillume.csproj
```

The project targets `net10.0-windows` and requires Windows Forms support.

## Configuration

Use the main window to configure:

1. Whether scheduled screenshots are enabled.
2. The capture interval.
3. Full-screen or active-window capture mode.
4. The destination folder.
5. The image format and, for JPG/WebP, image quality.
6. Notifications and Windows startup behavior.

Settings are stored in:

```text
%APPDATA%\Capillume\settings.json
```

The default destination is the `Capillume Screenshots` folder under the current user's Pictures directory.

## Technology

- C# and .NET 10
- Windows Forms
- `System.Drawing` for Windows screen capture and BMP output
- [SkiaSharp](https://github.com/mono/SkiaSharp) for PNG, JPG, and WebP encoding

## License

See the repository for licensing information.

## Project

[GitHub repository](https://github.com/dasdebjyoti/Capillume)
