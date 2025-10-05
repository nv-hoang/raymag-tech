import { createApp } from 'vue';
import mitt from 'mitt';
import axios from 'axios';
import dayjs from 'dayjs';
import VueTheMask from 'vue-the-mask'
import locale from './locale';
import { registerGlobalComponents } from './global-components';
import './style.css';

const app = createApp({
  setup() {
    const theme = {
      hashed: false,
      token: {
        fontFamily: '"Noto Sans TC", sans-serif',
        fontSize: 14,
        lineHeight: 1.5,

        colorPrimary: '#0895DA',
        colorTextBase: '#374151',
        colorPrimaryHover: 'none',

        colorLink: 'inherit',
        colorLinkActive: 'inherit',
        colorLinkHover: 'inherit',

        // controlOutline: 'none',
        controlOutlineWidth: 0,
      },
    };
    return { theme };
  },
  data() {
    return {
      sm: window.innerWidth <= 768,
      MaxUploadSize: window.MaxUploadSize || "50mb"
    };
  },
  mounted() {
    window.addEventListener('resize', () => this.sm = (window.innerWidth <= 768));
  },
  template: '<a-config-provider :theme="theme" :locale="$tm(\'antd\')">' + document.getElementById('app').innerHTML + '</a-config-provider>'
});

app.use(locale);
app.config.globalProperties.i18n = locale;
app.use(VueTheMask);

const emitter = mitt();
app.config.globalProperties.emitter = emitter;

const api = axios.create({
  baseURL: window.apiBaseURL,
  headers: { 'Content-Type': 'application/json' },
});
app.config.globalProperties.api = api;

app.config.globalProperties.dayjs = dayjs;
app.config.globalProperties.getVal = (obj, key) => key.split('.').reduce((acc, key) => acc?.[key], obj);

registerGlobalComponents(app);

app.mount('#app');
