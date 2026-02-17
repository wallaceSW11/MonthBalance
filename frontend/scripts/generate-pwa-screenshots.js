import sharp from 'sharp';
import { fileURLToPath } from 'url';
import { dirname, join } from 'path';

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);
const publicDir = join(__dirname, '..', 'public');

// Cores do tema
const darkBg = { r: 18, g: 18, b: 18 };
const primary = { r: 33, g: 150, b: 243 };

async function generateScreenshots() {
  console.log('📸 Generating PWA screenshots...\n');

  // Screenshot mobile (portrait)
  const mobileWidth = 390;
  const mobileHeight = 844;
  
  await sharp({
    create: {
      width: mobileWidth,
      height: mobileHeight,
      channels: 4,
      background: darkBg
    }
  })
  .composite([
    {
      input: Buffer.from(
        `<svg width="${mobileWidth}" height="${mobileHeight}">
          <rect width="100%" height="60" fill="rgb(33,150,243)"/>
          <text x="50%" y="35" text-anchor="middle" font-size="24" fill="white" font-family="Arial">Month Balance</text>
          <rect x="20" y="80" width="${mobileWidth - 40}" height="120" rx="8" fill="rgb(33,33,33)"/>
          <text x="40" y="120" font-size="18" fill="white" font-family="Arial">Previsão Financeira</text>
          <text x="40" y="160" font-size="32" fill="rgb(76,175,80)" font-family="Arial">R$ 5.420,00</text>
          <rect x="20" y="220" width="${mobileWidth - 40}" height="80" rx="8" fill="rgb(33,33,33)"/>
          <rect x="20" y="320" width="${mobileWidth - 40}" height="80" rx="8" fill="rgb(33,33,33)"/>
          <rect x="20" y="420" width="${mobileWidth - 40}" height="80" rx="8" fill="rgb(33,33,33)"/>
        </svg>`
      ),
      top: 0,
      left: 0
    }
  ])
  .png()
  .toFile(join(publicDir, 'screenshot-mobile.png'));

  console.log('✅ Generated screenshot-mobile.png (390x844)');

  // Screenshot desktop (landscape/wide)
  const desktopWidth = 1280;
  const desktopHeight = 720;
  
  await sharp({
    create: {
      width: desktopWidth,
      height: desktopHeight,
      channels: 4,
      background: darkBg
    }
  })
  .composite([
    {
      input: Buffer.from(
        `<svg width="${desktopWidth}" height="${desktopHeight}">
          <rect width="100%" height="64" fill="rgb(33,150,243)"/>
          <text x="40" y="42" font-size="24" fill="white" font-family="Arial">Month Balance</text>
          <rect x="40" y="100" width="580" height="140" rx="8" fill="rgb(33,33,33)"/>
          <text x="60" y="150" font-size="20" fill="white" font-family="Arial">Previsão Financeira Mensal</text>
          <text x="60" y="200" font-size="42" fill="rgb(76,175,80)" font-family="Arial">R$ 5.420,00</text>
          <rect x="660" y="100" width="580" height="140" rx="8" fill="rgb(33,33,33)"/>
          <text x="680" y="150" font-size="20" fill="white" font-family="Arial">Receitas</text>
          <text x="680" y="200" font-size="36" fill="rgb(76,175,80)" font-family="Arial">R$ 8.500,00</text>
          <rect x="40" y="270" width="580" height="400" rx="8" fill="rgb(33,33,33)"/>
          <text x="60" y="310" font-size="20" fill="white" font-family="Arial">Despesas Recentes</text>
          <rect x="660" y="270" width="580" height="400" rx="8" fill="rgb(33,33,33)"/>
          <text x="680" y="310" font-size="20" fill="white" font-family="Arial">Gráfico Mensal</text>
        </svg>`
      ),
      top: 0,
      left: 0
    }
  ])
  .png()
  .toFile(join(publicDir, 'screenshot-desktop.png'));

  console.log('✅ Generated screenshot-desktop.png (1280x720)');

  console.log('\n🎉 All screenshots generated successfully!');
}

generateScreenshots();
