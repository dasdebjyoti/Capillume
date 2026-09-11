# Copilot Instructions

## Project Guidelines
- For Capillume, watermarking and annotation should remain before downscaling because applying them after downscaling makes the watermark appear disproportionately large; do not move WatermarkRenderer.Apply after downscaling unless watermark dimensions and font sizes are scaled accordingly.