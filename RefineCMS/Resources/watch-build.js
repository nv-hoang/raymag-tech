// watch-build-copy.js
import { build } from 'vite';
import chokidar from 'chokidar';
import fs from 'fs-extra';
import path from 'path';

const distFolder = './dist';
const targetFolder = './../wwwroot/dist';

async function runBuildAndCopy() {
  console.log('🔨 Building project...');
  try {
    await build();
    console.log('✨ Build done. Copying files...');

    // Clear target folder and copy new dist contents
    await fs.emptyDir(targetFolder);
    await fs.copy(distFolder, targetFolder);

    console.log('📦 Copied dist to target folder:', targetFolder);
  } catch (err) {
    console.error('❌ Error during build or copy:', err);
  }
}

// Initial run
runBuildAndCopy();

const watcher = chokidar.watch('./src', {
  persistent: true,
  ignoreInitial: true,
  usePolling: true,            // <- make it more reliable
  interval: 100,               // <- polling interval (ms)
  awaitWriteFinish: {
    stabilityThreshold: 200,
    pollInterval: 100
  }
});

watcher.on('change', filePath => {
  console.log(`📝 File changed: ${filePath}`);
  runBuildAndCopy();
});

watcher.on('add', filePath => {
  console.log(`📄 File added: ${filePath}`);
  runBuildAndCopy();
});

watcher.on('unlink', filePath => {
  console.log(`🗑️ File removed: ${filePath}`);
  runBuildAndCopy();
});
