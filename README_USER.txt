CAPILLUME
Screenshot Automation for Windows
===============================

Capillume automatically captures screenshots and saves them to a folder of your
choice. It can capture all connected displays or the currently active window.

WHO CAPILLUME IS FOR
--------------------

The core user: people who need automatic proof of work
    - Remote workers and freelancers who need to show what they were working
      on, such as when a client requests screenshots every 10 minutes.
    - Students, exam proctors, and lab computers that need scheduled capture of
      lab or study activity.
    - QA testers and developers who need automatic screenshots of bug
      reproduction, long-running test progress, or a build time-lapse.

Documentation and compliance
    - Technical writers, IT support teams, and trainers who need timed,
      full-desktop screenshots for tutorials, SOPs, and training materials.
    - Office workers in regulated industries who need a local audit trail of
      what is on screen, with the knowledge and consent of the people involved.

Personal productivity and security
    - Power users who want a lightweight alternative to tools such as TimeSnap
      or Time Doctor, while keeping screenshots local and private.
    - People monitoring their own computer, such as a children's PC or a shared
      household PC.

Screenshots stay in your configured local folder, such as your Pictures folder;
they are not uploaded to a Capillume server.

WHO CAPILLUME IS NOT FOR
------------------------

    - One-off screenshot users. Windows Snipping Tool or Win+Shift+S is better
      suited for occasional screenshots.
    - Video recording. Capillume captures images only; it does not record video.
    - Stealth employee surveillance. Capillume is designed for transparent use
      with notifications, a visible system-tray presence, and user-accessible
      settings. Always obtain appropriate consent before capturing screens.

GETTING STARTED
---------------

1. Start Capillume from the Start menu or desktop shortcut.
2. Configure the capture options in the main window.
3. Select whether scheduled screenshots are enabled.
4. Set the capture interval in minutes.
5. Choose Full Screen or Active Window capture.
6. Choose whether to include the Capillume window in screenshots.
7. Select the folder where screenshots should be saved.
8. Select the image format.
9. Configure watermarks if needed.
10. Configure annotations if needed.
11. Configure downscaling if needed.
12. Click Save.
13. Use Capture Now at any time to save an immediate screenshot.

The default screenshot folder is:

%USERPROFILE%\Pictures\Capillume Screenshots

CAPTURE MODES
-------------

Full Screen
    Captures all connected displays.

Active Window
    Captures the currently active window.

Include Capillume in screenshots
    By default, Capillume does not include its own window in screenshots. Enable
    Include Capillume in screenshots to include it in both Full Screen and
    Active Window captures. If it is disabled, no screenshot is saved when the
    Capillume window is the active window.

Capture Now
    Click the Capture Now button in the main window to take an immediate
    screenshot. This action is available even when scheduled screenshots are
    disabled.

    To take an immediate screenshot while the application is minimized, right-
    click the Capillume icon in the Windows system tray and select Capture Now.

IMAGE FORMATS
-------------

Capillume supports:

- PNG
- JPG
- BMP
- WEBP

JPG and WEBP images include a quality setting. Higher quality produces larger
files.

WATERMARKS
----------

You can configure text and image watermarks from the Watermark tab in the Advanced Settings window.

Text watermarks support:

- Custom text
- Font family
- Font size
- Font style
- Opacity
- Position
- Rotation

Image watermarks support:

- PNG, JPG, JPEG, BMP, and GIF files
- Image scale
- Opacity
- Position
- Rotation

Watermarks are applied before screenshots are saved.

ANNOTATIONS
-----------

You can configure text annotations from the Annotation tab in the Advanced
Settings window. Annotations lets you add dynamic information to captured
screenshots. They appear at the bottom center of the image and support
custom formats, font family, font size, font style, text color, optional
background highlighting, and opacity. Use the annotation field menu to insert
supported fields into the format.

Insert one or more of these fields into the annotation format:

    {{DATE}}         Local date (yyyy-MM-dd)
    {{TIME}}         Local time (HH:mm:ss)
    {{DATETIME}}     Local date and time
    {{UTC}}          UTC date and time
    {{TIMEZONE}}     Local time zone
    {{OFFSET}}       Local UTC offset
    {{MILLISECONDS}} Milliseconds component
    {{PCNAME}}       Computer name
    {{USER}}         Windows user name
    {{OS}}           Operating system version
    {{APP}}          Application name
    {{VERSION}}      Application version
    {{PID}}          Process ID

The default annotation format is:

    {{OS}} | {{DATETIME}}

DOWNSCALING
-----------

You can configure screenshot downscaling from the Downscale tab in the Advanced
Settings window.

Downscale modes:

- Target Height: Resize using a fixed output height.
- Percentage: Resize to a percentage of the original capture size.
- Max Width: Reduce to a maximum width.
- Fit Within Bounding Box: Fit inside a configured width and height.

Downscale options:

- Quality: High Quality (Bicubic), Balanced (Bilinear), or Fast (Nearest Neighbor)
- Sharpen after resize
- Skip smaller images
- Full-screen only
- Lossy formats only (JPG/WEBP)

SYSTEM TRAY
-----------

When the main window is closed, Capillume continues running in the Windows
system tray.

Right-click the Capillume tray icon to use these commands:

- Show: Opens the main window.
- Capture Now: Takes an immediate screenshot.
- About: Displays application information.
- Exit: Stops Capillume and closes the application completely.

Double-click the tray icon to open the main window.

START WITH WINDOWS
------------------

Enable Start with Windows in the main window if Capillume should start
automatically when you sign in to Windows.

When started automatically, Capillume runs in the system tray.

PAUSING AND RESUMING
--------------------

Capillume automatically pauses scheduled capture when:

- The Windows session is locked.
- The computer enters sleep or suspend mode.

Capture resumes after the session is unlocked or the computer wakes up. A
screenshot is taken immediately when capture resumes, and the normal schedule
continues afterward.

During Windows logoff or shutdown, Capillume stops scheduling new screenshots
and exits gracefully.

SETTINGS
--------

Settings are stored for the current Windows user at:

%APPDATA%\Capillume\settings.json

If the destination folder does not exist, Capillume creates it when the first
screenshot is taken.

PRIVACY
-------

Screenshots are saved locally to the configured destination folder. Capillume
does not upload screenshots or send them to an online service.

REQUIREMENTS
------------

- Windows 10 or later.
- .NET 10 runtime with Windows desktop support.
- A Windows user account with permission to capture the desktop and write to
  the selected destination folder.

TROUBLESHOOTING
---------------

Capillume is already running
    Capillume allows only one running instance. Check the Windows system tray
    for the Capillume icon.

The main window disappeared
    Capillume normally minimizes to the system tray when the window is closed.
    Double-click the tray icon or select Show from the tray menu.

Screenshots are not being saved
    Check that scheduled capture is enabled, the destination folder exists or
    can be created, and the current user has write permission to that folder.

The selected watermark cannot be used
    Confirm that the watermark image still exists and that it is a supported
    image format.

SUPPORT
-------

Project:
https://github.com/dasdebjyoti/Capillume

Copyright © 2026 Capillume.