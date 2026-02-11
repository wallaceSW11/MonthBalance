import sharp from 'sharp';
import { fileURLToPath } from 'url';
import { dirname, join } from 'path';
import { existsSync } from 'fs';

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);
const publicDir = join(__dirname, '..', 'public');

const sourceImage = join(publicDir, 'logo-dark.png');

if (!existsSync(sourceImage)) {
  console.error('❌ Source image not found:', sourceImage);
  process.exit(1);
}

const sizes = [
  { size: 192, name: 'pwa-192x192.png' },
  { size: 512, name: 'pwa-512x512.png' },
  { size: 180, name: 'apple-touch-icon.png' }
];

async function generateIcons() {
  console.log('🎨 Generating PWA icons...\n');

  for (const { size, name } of sizes) {
    const outputPath = join(publicDir, name);

    try {
      await sharp(sourceImage)
        .resize(size, size, {
          fit: 'contain',
          background: { r: 0, g: 0, b: 0, alpha: 0 }
        })
        .png()
        .toFile(outputPath);

      console.log(`✅ Generated ${name} (${size}x${size})`);
    } catch (error) {
      console.error(`❌ Failed to generate ${name}:`, error.message);
      process.exit(1);
    }
  }

  console.log('\n🎉 All icons generated successfully!');
}

generateIcons();
