import { defineComponent } from 'vue';
import {
  ConfigProvider,
  Input,
  Checkbox,
  Radio,
  Select,
  Switch,
  Button,
  Dropdown,
  Menu,
  Alert,
  Avatar,
  Collapse,
  Tabs,
  Popover,
  Tooltip,
  Table,
  Tag,
  Slider,
  Space,
  DatePicker,
  Modal,
  Form,
  Popconfirm,
  Row,
  Col
} from 'ant-design-vue';
const antComponents = {
  ConfigProvider,
  Input,
  Checkbox,
  Radio,
  Select,
  Switch,
  Button,
  Dropdown,
  Menu,
  Alert,
  Avatar,
  Collapse,
  Tabs,
  Popover,
  Tooltip,
  Table,
  Tag,
  Slider,
  Space,
  DatePicker,
  Form,
  Popconfirm,
  Row,
  Col
}

import {
  ArrowLeftOutlined,
  LeftOutlined,
  GlobalOutlined,
  MenuOutlined,
  UserOutlined,
  LogoutOutlined,
  AppstoreOutlined,
  HomeOutlined,
  BookOutlined,
  QuestionCircleOutlined,
  DashboardOutlined,
  FilterOutlined,
  DownOutlined,
  FileOutlined,
  CloseOutlined,
  CloseCircleOutlined,
  PlusCircleFilled,
  EditOutlined,
  CheckCircleOutlined,
  CheckOutlined
} from '@ant-design/icons-vue';
const icons = {
  ArrowLeftOutlined,
  LeftOutlined,
  GlobalOutlined,
  MenuOutlined,
  UserOutlined,
  LogoutOutlined,
  AppstoreOutlined,
  HomeOutlined,
  BookOutlined,
  QuestionCircleOutlined,
  DashboardOutlined,
  FilterOutlined,
  DownOutlined,
  FileOutlined,
  CloseOutlined,
  CloseCircleOutlined,
  PlusCircleFilled,
  EditOutlined,
  CheckCircleOutlined,
  CheckOutlined
}

import { PerfectScrollbarPlugin } from 'vue3-perfect-scrollbar';
import 'vue3-perfect-scrollbar/style.css';

import Quill from 'quill';
import 'quill/dist/quill.snow.css';

import ImageUploader from 'quill-image-uploader';
import 'quill-image-uploader/dist/quill.imageUploader.min.css';

class CustomImageUploader extends ImageUploader {
  constructor(quill, options) {
    super(quill, options);
   
    quill.on('text-change', (delta, oldDelta, source) => {
      if (source === 'user') {
        const insertedBase64Image = (delta && delta.ops || []).some(
          op => op.insert && op.insert.image && typeof op.insert.image === 'string' && op.insert.image.indexOf('data:') === 0
        );
        if (insertedBase64Image) {
          // schedule removal after internals finish
          setTimeout(() => this.removeBase64Images(quill), 0);
        } else {
          this.onChangeContent();
        }
      }
    });
  }

  onChangeContent()
  {
    this.quill.options.modules.toolbar.handlers.onChangeContent(this.quill.root.innerHTML);
  }

  removeBase64Images(quill) {
    const delta = quill.getContents();
    const ops = delta.ops || [];
    let index = 0;
    const toDelete = [];

    for (let i = 0; i < ops.length; i++) {
      const op = ops[i];
      if (typeof op.insert === 'string') {
        index += op.insert.length;
      } else if (op.insert && op.insert.image) {
        const src = op.insert.image;
        if (typeof src === 'string' && src.indexOf('data:') === 0) {
          toDelete.push(index);
        }
        index += 1; // embeds count as length 1
      } else {
        // other embed types (video, formula, etc.)
        index += 1;
      }
    }

    // Delete from last to first so earlier indexes are unaffected
    for (let i = toDelete.length - 1; i >= 0; i--) {
      quill.deleteText(toDelete[i], 1);
    }

    setTimeout(() => this.onChangeContent(), 0);
  }

  insertBase64Image(url) { }
  removeBase64Image() { }

  insertToEditor(url) {
    const range = this.range;
    this.quill.insertEmbed(range.index, "image", `${url}`, "user");
    range.index++;
    this.quill.setSelection(range, "user");
  }
}
Quill.register("modules/imageUploader", CustomImageUploader);

import QuillResizeImage from 'quill-resize-image';
Quill.register('modules/resize', QuillResizeImage);

const ImageFormat = Quill.import('formats/image');
class ImageStyle extends ImageFormat {
  static formats(domNode) {
    return {
      style: domNode.getAttribute('style') || null,
    };
  }
  format(name, value) {
    if (name === 'style' && value) {
      this.domNode.setAttribute('style', value);
    } else {
      super.format(name, value);
    }
  }
}
Quill.register(ImageStyle, true);

import 'vuefinder/dist/style.css';
import VueFinder from 'vuefinder/dist/vuefinder';
import en from 'vuefinder/dist/locales/en.js'
import zhTW from 'vuefinder/dist/locales/zhTW.js';

export function registerGlobalComponents(app) {
  let i18n = app.config.globalProperties.i18n.global;

  for (const componentName in antComponents) {
    app.use(antComponents[componentName])
  }

  for (const iconName in icons) {
    app.component(iconName, icons[iconName]);
  }

  app.use(PerfectScrollbarPlugin);

  app.config.globalProperties.Quill = Quill;

  app.config.globalProperties.useConfirm = (title, content) => (new Promise((resolve, reject) => {
    let status = false;
    Modal.confirm({
      title: title || i18n.t('confirm.title'),
      content: content || i18n.t('confirm.desc'),
      onOk() {
        status = true;
      },
      afterClose() {
        if (status) resolve(status);
      }
    });
  }));

  // Local components
  const components = import.meta.glob('./components/**/*.{vue,js}', { eager: true });

  for (const path in components) {
    const component = components[path].default;
    const componentName = path
      .split('/')
      .slice(2)
      .map(word => word.charAt(0).toUpperCase() + word.slice(1))
      .join('')
      .replace(/\.\w+$/, ''); // Extract file name as component name

    app.component(componentName, defineComponent(component));
  }

  // Languge
  app.config.globalProperties.emitter.on('lang', (lang) => {
    const url = new URL(window.location.href);
    url.searchParams.set('lang', lang);
    window.location.href = url.toString();
  });

  // VueFinder language
  app.use(VueFinder, { i18n: vfinder_i18n });
  var vfinder_i18n = { en, zhTW };
  var lang = document.documentElement.lang.replaceAll('-', '');
  if (vfinder_i18n[lang]) {
    const storage = JSON.parse(localStorage.getItem('vf_storage') || '{}');
    storage.locale = lang;
    storage.translations = vfinder_i18n[lang];
    localStorage.setItem('vf_storage', JSON.stringify(storage));
  }

}
