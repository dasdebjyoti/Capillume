# Capillume

Capillume is a lightweight Windows screenshot automation tool. Capture your entire desktop or the active window on a schedule, save images in your preferred format, and keep the application available from the system tray.

## Features

- Capture all connected displays or the active window.
- Capture a screenshot immediately when scheduled capture starts.
- Configure the capture interval in minutes.
- Save screenshots to a custom folder.
- Save as PNG, JPG, BMP, or WebP.
- Configure JPG and WebP image quality.
- Add text and/or image watermarks to captured screenshots.
- Customize watermark text, font family, font size, and font style.
- Select a watermark image from a PNG, JPG, JPEG, BMP, or GIF file.
- Adjust watermark opacity and image scale.
- Position watermarks at the top, center, or bottom of the screenshot, aligned left, center, or right.
- Rotate watermarks by 0°, 90°, 180°, or 270°.
- Add dynamic annotations containing capture time, system, user, application, and process information.
- Show desktop notifications after screenshots are saved.
- Start automatically with Windows.
- Use the **Capture Now** command from the system tray.
- Minimize to the system tray instead of closing the application.
- Pause screenshot capture when Windows locks the session.
- Resume screenshot capture when the user unlocks the session.
- Pause screenshot capture while Windows is sleeping or suspended.
- Resume screenshot capture after Windows wakes up.
- Stop scheduling new captures and close gracefully during Windows logoff or shutdown.
- Complete an already-running synchronous capture before exiting.

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
6. Watermark settings.
7. Annotation settings.
8. Notifications and Windows startup behavior.

### Annotations

Open **Annotation** from the main window to add dynamic information to each
captured screenshot. Annotations are rendered at the bottom center and support
custom text formats, font settings, text color, optional highlighting, and
opacity.

Use these fields in an annotation format:

| Field | Value |
| --- | --- |
| `{{DATE}}` | Local date (`yyyy-MM-dd`) |
| `{{TIME}}` | Local time (`HH:mm:ss`) |
| `{{DATETIME}}` | Local date and time |
| `{{UTC}}` | UTC date and time |
| `{{TIMEZONE}}` | Local time zone |
| `{{OFFSET}}` | Local UTC offset |
| `{{MILLISECONDS}}` | Milliseconds component |
| `{{PCNAME}}` | Computer name |
| `{{USER}}` | Windows user name |
| `{{OS}}` | Operating system version |
| `{{APP}}` | Application name |
| `{{VERSION}}` | Application version |
| `{{PID}}` | Process ID |

The default annotation format is `{{OS}} | {{DATETIME}}`.

### Watermarks

Open **Watermark** from the main window to configure watermarking. Text and
image watermarks can be used independently or together. Text watermarks support
a custom font and style, while image watermarks support scaling from 1% to 100%.
Both watermark types support opacity from 1% to 100%, nine placement options,
and 0°, 90°, 180°, or 270° rotation.

Watermark settings are applied to each captured screenshot before it is saved.
If a watermark image is enabled, the selected image file must exist when the
settings are saved.

Capture automatically pauses during screen lock and system sleep. These lifecycle
behaviors are enabled by default and do not require a separate dialog. The
**Exit** command closes Capillume completely; closing the main window normally
minimizes the application to the system tray.

Capillume does not capture while the Windows session is locked or the computer
is suspended. When capture resumes after unlock or wake, one screenshot is
taken immediately and the configured schedule continues.

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

## Privacy and lifecycle behavior

Screenshots are saved locally to the configured destination folder. Capture is
automatically paused when the screen is locked or Windows enters sleep, which
helps prevent lock-screen or unattended-session screenshots. During logoff and
shutdown, Capillume stops scheduling new captures and exits without minimizing
to the tray.

## License

See the repository for licensing information.

## Project

[GitHub repository](https://github.com/dasdebjyoti/Capillume)
