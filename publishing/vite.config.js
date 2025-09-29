import { defineConfig } from "vite";
import { glob } from 'glob';
import path from 'node:path';
import { fileURLToPath } from 'node:url';
import handlebars from 'vite-plugin-handlebars';

// https://vitejs.dev/config/
export default defineConfig({
    root: path.resolve(__dirname, 'src'),
    publicDir: path.resolve(__dirname, 'public'),
    build: {
        rollupOptions: {
            input: Object.fromEntries(
                glob.sync('src/*.html').map(file => [
                    file.slice(4, file.length - path.extname(file).length),
                    fileURLToPath(new URL(file, import.meta.url))
                ])
            ),
            output: {
                dir: path.resolve(__dirname, 'dist'),
                entryFileNames: `assets/main.js`,
                assetFileNames: (assetInfo) => {
                    if (assetInfo.name && assetInfo.name.endsWith('.css')) {
                        return 'assets/main.css';
                    }
                    return `assets/[name].[ext]`;
                },
            },
        },
    },
    plugins: [
        handlebars({
            partialDirectory: path.resolve(__dirname, 'src/partials'),
        }),
    ],
});
