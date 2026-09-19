# Capillume

Capillume is a lightweight Windows screenshot automation tool. Capture your entire desktop or the active window on a schedule, save images in your preferred format, and keep the application available from the system tray.

## Features

- Capture all connected displays or the active window.
- Exclude the Capillume window from screenshots by default, or include it when needed.
- Capture a screenshot immediately when scheduled capture starts.
- Configure the capture interval in minutes.
- Save screenshots to a custom folder.
- Save as PNG, JPG, BMP, or WebP.
- Configure JPG and WebP image quality.
- Downscale screenshots before saving using target height, resize percentage, max width, or bounding box modes.
- Choose downscale quality and optional processing controls such as sharpen, skip smaller images, full-screen only, and lossy-formats only.
- Process screenshots before saving with color modes, high contrast, noise reduction, and color temperature adjustments.
- Add text and/or image watermarks to captured screenshots.
- Customize watermark text, font family, font size, and font style.
- Select a watermark image from a PNG, JPG, JPEG, BMP, or GIF file.
- Adjust watermark opacity and image scale.
- Position watermarks at the top, center, or bottom of the screenshot, aligned left, center, or right.
- Rotate watermarks by 0°, 90°, 180°, or 270°.
- Add dynamic annotations containing capture time, system, user, application, and process information.
- Automatically clean up screenshots by age and/or file count.
- Move retained screenshots to the Recycle Bin, delete them permanently, or back them up before deletion.
- Show desktop notifications after screenshots are saved.
- Start automatically with Windows.
- Use the **Capture Now** button in the main window for an immediate screenshot.
- Use the **Capture Now** command from the system tray.
- Configure an optional global keyboard shortcut to capture a screenshot while Capillume is minimized or another application is active.
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

To create the distributable Windows build, run:

```powershell
dotnet build .\Capillume.csproj --configuration Release
```

The Release build publishes a self-contained, single-file `win-x64` application
under `bin\Release\net10.0-windows\publish\win-x64`. The published output includes
`LICENSE.txt` and `README.txt` for distribution with the application.

## Configuration

Use the main window to configure:

1. Whether scheduled screenshots are enabled.
2. The capture interval.
3. Full-screen or active-window capture mode.
4. Whether the Capillume window is included in screenshots.
5. The destination folder.
6. The image format and, for JPG/WebP, image quality.
7. Watermark settings.
8. Annotation settings.
9. Downscale settings.
10. Image Processing settings.
11. Retention and cleanup settings.
12. Notifications and Windows startup behavior.
13. An optional global capture hotkey.

### Watermarks

Go to **Watermark** tab in the Advanced Settings window to configure
watermarking. Text and image watermarks can be used independently or
together. Text watermarks support a custom font and style, while image
watermarks support scaling from 1% to 100%. Both watermark types support
opacity from 1% to 100%, nine placement options, and 0°, 90°, 180°, or 270°
rotation.

Watermarks and annotations are applied to each captured screenshot before
downscaling. If downscaling is enabled, image processing is applied after
downscaling and before the result is saved. If a watermark image is enabled, the selected image file must exist when the settings are saved.

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

The default destination is the `Capillume Screenshots` folder under the current
user's Pictures directory. Scheduled capture, save notifications, Windows
startup, and the global hotkey are disabled by default.

### Capture scope

By default, Capillume does not include its own window in screenshots. Enable
**Include Capillume in screenshots** to include it in both full-screen and
active-window captures. For active-window capture, screenshots are not saved
when the Capillume window is the active window while this option is disabled.

### Hotkeys

Go to the **Hotkeys** tab in the Advanced Settings window to enable or disable
the global **Capture Now** shortcut. Choose at least one modifier key (Ctrl,
Shift, Alt, or Windows) and a keyboard key. The default shortcut shown when no
shortcut has been configured is **Ctrl+Shift+F12**, but the global hotkey is
disabled by default.

The shortcut works while Capillume is minimized to the system tray or another
application is active. Capillume checks whether the shortcut is available when
you save the settings. If Windows or another application is already using it,
the settings are not applied and the previous shortcut remains active.

### About

Open **About** from the main window or the system-tray menu to view the installed
application version and open the Capillume GitHub repository.

### Annotations

Go to **Annotation** tab in the Advanced Settings window to add dynamic information
to each captured screenshot. Annotations are rendered at the bottom center and
support custom text formats, font family, font size, font style, text color,
optional background highlighting, and opacity. Use the annotation field menu to
insert supported placeholders into the format.

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

### Downscaling

Go to **Downscale** tab in the Advanced Settings window to reduce screenshot
size before saving. Enable downscaling and choose one mode:

- **Target Height**: resizes using a fixed height.
- **Percentage**: resizes to a percentage of the original capture.
- **Max Width**: reduces width to a maximum value.
- **Fit Within Bounding Box**: fits the image inside a width/height box.

You can also choose resize quality (**High Quality/Bicubic**, **Balanced/Bilinear**,
or **Fast/Nearest Neighbor**) and optionally enable sharpen-after-resize, skip
smaller images, full-screen-only downscaling, or lossy-formats-only downscaling.

### Image Processing

Go to the **Image Processing** tab in the Advanced Settings window to adjust the
appearance and color depth of captured screenshots. Choose one color mode:

- **Full color**: preserves the original colors.
- **Grayscale mode**: converts the image to shades of gray.
- **Monochrome (1-bit)**: converts the image to black and white.
- **16-color**: reduces the image to a 16-color palette.
- **256-color**: reduces the image to a 256-color palette.
- **Adaptive palette (quantization)**: creates a palette of up to 256 colors
  based on the captured image.

The Advanced Processing options are:

- **High contrast**: increases tonal contrast.
- **Noise reduction**: applies a light blur to reduce small image variations.
- **Color temperature**: applies a **Neutral**, **Warm**, or **Cool** color tone.

Image processing is applied after downscaling and before the screenshot is saved.
All image processing options are disabled by default.

### Retention

Go to the **Retention** tab in the Advanced Settings window to control cleanup
of saved screenshots. Automatic cleanup runs after a screenshot is saved when
**Enable File Auto-cleanup** is enabled. Cleanup considers JPG, JPEG, PNG, BMP,
and WebP files in the destination folder. Enable **Include subfolders** to scan
the folder tree as well.

Retention rules are applied in this order:

1. Remove screenshots older than the configured maximum age.
2. From the files that remain, retain only the newest configured number of screenshots.

The default retention settings are a 30-day maximum age and a 500-screenshot
file-count limit. The age rule is enabled by default; the file-count rule is
available but disabled by default. Cleanup can:

- Move files to the Windows Recycle Bin.
- Delete files permanently.
- Back up files to another folder before deleting them, optionally creating a
  separate subfolder for each cleanup session.

Use **Cleanup Now** to preview or execute cleanup immediately. Enable **Dry run
mode** to preview candidates and the estimated space to be freed without
deleting or recycling files.

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
