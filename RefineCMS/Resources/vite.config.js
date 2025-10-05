import { defineConfig } from "vite";
import { globSync } from 'glob';
import path from 'node:path';
import { fileURLToPath } from 'node:url';
import handlebars from 'vite-plugin-handlebars';
import vue from '@vitejs/plugin-vue'
import tailwindcss from '@tailwindcss/vite'
import Components from 'unplugin-vue-components/vite'
import { AntDesignVueResolver } from 'unplugin-vue-components/resolvers'

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    vue(),
    tailwindcss(),
    Components({
      resolvers: [
        AntDesignVueResolver({
          importStyle: false, // css in js
        }),
      ],
    }),
    handlebars({
      partialDirectory: path.resolve(__dirname, 'src/partials'),
    }),
  ],
  resolve: {
    alias: {
      'vue': 'vue/dist/vue.esm-bundler.js'
    }
  },
  root: path.resolve(__dirname, 'src'),
  publicDir: path.resolve(__dirname, 'public'),
  build: {
    chunkSizeWarningLimit: 512000,
    rollupOptions: {
      input: Object.fromEntries(
        globSync('src/*.html').map(file => [
          file.slice(4, file.length - path.extname(file).length),
          fileURLToPath(new URL(file, import.meta.url))
        ])
      ),
      output: {
        dir: path.resolve(__dirname, 'dist'),
        entryFileNames: 'assets/[name].js',
        chunkFileNames: 'assets/[name].js',
        assetFileNames: (assetInfo) => {
          if (assetInfo.name && assetInfo.name.endsWith('.css')) {
            return 'assets/main.css';
          }
          return `assets/[name].[ext]`;
        },
      },
    },
  },
})
