import { createI18n } from 'vue-i18n';
import en_US from 'ant-design-vue/es/locale/en_US';
import zh_TW from 'ant-design-vue/es/locale/zh_TW';

import datePicker_en_US from 'ant-design-vue/es/date-picker/locale/en_US';
import datePicker_zh_TW from 'ant-design-vue/es/date-picker/locale/zh_TW';

const messages = {
  en: {
    antd: en_US,
    datePicker: datePicker_en_US,
    total: 'Total: {0}',
    generate: 'Generate',
    edit: 'Edit',
    save: 'Save',
    cancel: 'Cancel',
    clear: 'Clear',
    delete: 'Delete',
    backToList: 'Back to List',
    inputRequired: 'This field is required.',
    choose_file: 'Choose file',
    download: 'Download',
    deleteConfirm: {
      title: 'Are you sure delete it?',
      yes: 'Yes',
      no: 'No'
    },
    confirm: {
      title: 'Confirm?',
      desc: 'Are you sure you want to proceed with this action?'
    },
    editor: {
      header: {
        heading: 'Heading {0}',
        normal: 'Normal',
      },
      video: {
        enterVideo: 'Enter video:',
      },
      link: {
        enterLink: 'Enter link:',
        visitURL: 'Visit URL:',
        save: 'Save',
        edit: 'Edit',
        remove: 'Remove',
      },
      imageResize: {
        floatLeft: "left",
        floatRight: "right",
        center: "center",
        restore: "restore",
        altTip: "Press and hold alt to lock ratio!",
        inputTip: "Press enter key to apply change!",
      },
    },
    repeater: {
      addRow: 'Add Row',
      delRow: 'Delete Row',
      insertRow: 'Insert Row',
    },
  },
  "zh-CN": {
    antd: zh_TW,
    datePicker: datePicker_zh_TW,
    total: '总计: {0}',
    generate: '生成',
    edit: '编辑',
    save: '保存',
    cancel: '取消',
    clear: '清除',
    delete: '删除',
    backToList: '返回列表',
    inputRequired: '此字段为必填项。',
    choose_file: '选择文件',
    download: '下载',
    deleteConfirm: {
      title: '您确定要删除它吗？',
      yes: '是',
      no: '否'
    },
    confirm: {
      title: '确认吗？',
      desc: '您确定要继续执行此操作吗？'
    },
    editor: {
      header: {
        heading: '标题 {0}',
        normal: '正常',
      },
      video: {
        enterVideo: '输入视频:',
      },
      link: {
        enterLink: '输入链接:',
        visitURL: '访问网址:',
        save: '保存',
        edit: '编辑',
        remove: '移除',
      },
      imageResize: {
        floatLeft: "左侧",
        floatRight: "右侧",
        center: "居中",
        restore: "恢复",
        altTip: "按住 Alt 键以锁定比例！",
        inputTip: "按 Enter 键以应用更改！",
      },
    },
    repeater: {
      addRow: '添加行',
      delRow: '删除行',
      insertRow: '插入行',
    },
  },
  "zh-TW": {
    antd: zh_TW,
    datePicker: datePicker_zh_TW,
    total: '總計: {0}',
    generate: '生成',
    edit: '編輯',
    save: '儲存',
    cancel: '取消',
    clear: '清除',
    delete: '刪除',
    backToList: '返回列表',
    inputRequired: '此欄位為必填。',
    choose_file: '選擇檔案',
    download: '下載',
    deleteConfirm: {
      title: '你確定要刪除它嗎？',
      yes: '是',
      no: '否'
    },
    confirm: {
      title: '確認嗎？',
      desc: '您確定要執行此操作嗎？'
    },
    editor: {
      header: {
        heading: '標題 {0}',
        normal: '一般',
      },
      video: {
        enterVideo: '輸入影片:',
      },
      link: {
        enterLink: '輸入連結:',
        visitURL: '造訪網址:',
        save: '儲存',
        edit: '編輯',
        remove: '移除',
      },
      imageResize: {
        floatLeft: "靠左",
        floatRight: "靠右",
        center: "置中",
        restore: "還原",
        altTip: "按住 Alt 鍵以鎖定比例！",
        inputTip: "按 Enter 鍵以套用更改！",
      },
    },
    repeater: {
      addRow: '新增列',
      delRow: '刪除列',
      insertRow: '插入列',
    },
  },
};

const datetimeFormats = Object.fromEntries(
  Object.keys(messages).map(key => [key, {
    at: {
      year: 'numeric', month: 'numeric', day: 'numeric',
      weekday: 'short', hour: 'numeric', minute: 'numeric', hour12: true
    }
  }])
);

export default createI18n({
  locale: document.documentElement.lang,
  fallbackLocale: 'en',
  messages,
  datetimeFormats: datetimeFormats,
  globalInjection: true,
});