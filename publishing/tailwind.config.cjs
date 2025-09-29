/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./src/**/*.html"],
    theme: {
        fontFamily: {
            body: ["Inter", "Noto Sans TC", "sans-serif"],
        },
        colors: {
            black: '#000000',
            white: "#ffffff",
            transparent: 'transparent',
            primary: {
                300: '#A0CDE4',
                500: '#0895DA',
                900: '#030A11',
            },
            gray: {
                100: '#E7E7E7',
                500: '#6D6D6D',
            }
        },
        extend: {
            lineHeight: {
                '100': '100%',
                '110': '110%',
                '120': '120%',
                '125': '125%',
                '130': '130%',
                '140': '140%',
                '150': '150%',
                '160': '160%',
                '170': '170%',
                '175': '175%',
            },
        },
    },
    plugins: [
        function ({ addBase, theme }) {
            function extractColorVars(colorObj, colorGroup = '') {
                return Object.keys(colorObj).reduce((vars, colorKey) => {
                    const value = colorObj[colorKey];

                    const newVars =
                        typeof value === 'string'
                            ? { [`--tw${colorGroup}-${colorKey}`]: value }
                            : extractColorVars(value, `-${colorKey}`);

                    return { ...vars, ...newVars };
                }, {});
            }

            addBase({
                ':root': extractColorVars(theme('colors')),
            });
        },
    ],
};
