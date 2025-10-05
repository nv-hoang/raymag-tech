<template>
  <a-form-item :help="getMessage()" :validate-status="getMessage() ? 'error':''">
    <div :style="disabled ? 'pointer-events: none; opacity: 0.5;':''">
      <div :style="getStyle()">
        <div ref="editor">
          <slot>
            <div v-html="fvalue"></div>
          </slot>
        </div>
      </div>
      
      <Finder v-model:open="finder" :image-only="true" @selected="(url) => insertImage(url)"></Finder>
    </div>
  </a-form-item>
</template>

<script>
import Finder from "./finder.vue";
import base from './base';

export default {
  mixins: [base],
  components: {
    Finder
  },
  props: {
    uploadapi: { type: String, default: '/admin/storage/editor-upload' },
  },
  data() {
    return { 
      editor: null,
      finder: false,
      insertImage: null,
    }
  },
  watch: {
    fvalue(val, oldVal) {
      if (val != this.getContent()) {
        this.$refs.editor.querySelector('.ql-editor').innerHTML = val;
      }
    },
  },
  methods: {
    isEmpty() {
      const div = document.createElement('div');
      div.innerHTML = this.getContent();
      return (div.textContent || div.innerText || '') == '';
    },
    getContent() {
      return this.$refs.editor.querySelector('.ql-editor').innerHTML;
    },
    getStyle() {
      var style = [1, 2, 3, 4, 5, 6, false].map(i => {
        if (i === false) return `--ql-p: '${this.$t('editor.header.normal')}';`;
        return `--ql-h${i}: '${this.$t('editor.header.heading', [i])}';`;
      }).join('');

      style += `--ql-enterLink: '${this.$t('editor.link.enterLink')}';`;
      style += `--ql-visitURL: '${this.$t('editor.link.visitURL')}';`;
      style += `--ql-save: '${this.$t('editor.link.save')}';`;
      style += `--ql-edit: '${this.$t('editor.link.edit')}';`;
      style += `--ql-remove: '${this.$t('editor.link.remove')}';`;

      style += `--ql-enterVideo: '${this.$t('editor.video.enterVideo')}';`;

      return style;
    },
  },
  mounted() {
    this.editor = new this.Quill(this.$refs.editor, {
      theme: 'snow',
      modules: {
        toolbar: {
          container: [
            [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
            ['bold', 'italic', 'underline', 'link', 'clean'],
            [{ 'color': [] }, { 'background': [] }],
            [{ 'align': [] }],
            [{ 'list': 'ordered' }, { 'list': 'bullet' }],
            ['video', 'image2']
          ],
          handlers: {
            image2: function() {
              this.handlers.finder((url) => {
                this.quill.focus();
                const range = this.quill.getSelection();
                this.quill.insertEmbed(range.index, "image", url, "user");
                range.index++;
                this.quill.setSelection(range, "user");
              });
            },
            finder: (insertImage) => {
              this.insertImage = insertImage;
              this.finder = true;
            },
            onChangeContent: (content) => {
              this.change(content);
            }
          }
        },
        imageUploader: {
          upload: (file) => new Promise((resolve, reject) => {

            var fileUpload = new File([file], Date.now() + '-' + file.name, {
              type: file.type,
              lastModified: file.lastModified
            });

            const formData = new FormData();
            formData.append('name', fileUpload.name);
            formData.append('type', fileUpload.type);
            formData.append('file', fileUpload);

            this.api.post(this.uploadapi, formData, {
                params: {
                    q: 'upload',
                    adapter: 'local',
                    path: 'local://'
                },
                headers: {
                  'Content-Type': 'multipart/form-data'
                }
            }).then(res => {
                resolve(res.data.path);
            });
          }),
        },
        resize: { locale: this.$tm('editor.imageResize') },
      },
    });

    this.editor.on('text-change', (delta, oldDelta, source) => {
      this.change(this.getContent());
    });

    // this.editor.getModule("toolbar").addHandler('finder', (insertImage) => {
    //   this.insertImage = insertImage;
    //   this.finder = true;
    // });
  }
}
</script>

<style>
.ql-editor {
  min-height: 100px;
}

.ql-snow .ql-picker.ql-header .ql-picker-label::before,
.ql-snow .ql-picker.ql-header .ql-picker-item::before {
  content: var(--ql-p) !important;
}

.ql-snow .ql-picker.ql-header .ql-picker-label[data-value="1"]::before,
.ql-snow .ql-picker.ql-header .ql-picker-item[data-value="1"]::before {
  content: var(--ql-h1) !important;
}

.ql-snow .ql-picker.ql-header .ql-picker-label[data-value="2"]::before,
.ql-snow .ql-picker.ql-header .ql-picker-item[data-value="2"]::before {
  content: var(--ql-h2) !important;
}

.ql-snow .ql-picker.ql-header .ql-picker-label[data-value="3"]::before,
.ql-snow .ql-picker.ql-header .ql-picker-item[data-value="3"]::before {
  content: var(--ql-h3) !important;
}

.ql-snow .ql-picker.ql-header .ql-picker-label[data-value="4"]::before,
.ql-snow .ql-picker.ql-header .ql-picker-item[data-value="4"]::before {
  content: var(--ql-h4) !important;
}

.ql-snow .ql-picker.ql-header .ql-picker-label[data-value="5"]::before,
.ql-snow .ql-picker.ql-header .ql-picker-item[data-value="5"]::before {
  content: var(--ql-h5) !important;
}

.ql-snow .ql-picker.ql-header .ql-picker-label[data-value="6"]::before,
.ql-snow .ql-picker.ql-header .ql-picker-item[data-value="6"]::before {
  content: var(--ql-h6) !important;
}

.ql-snow .ql-tooltip[data-mode=link]::before {
  content: var(--ql-enterLink) !important;
}

.ql-snow .ql-tooltip.ql-editing a.ql-action::after {
  content: var(--ql-save) !important;
}

.ql-snow .ql-tooltip::before {
  content: var(--ql-visitURL) !important;
}

.ql-snow .ql-tooltip a.ql-action::after {
  content: var(--ql-edit) !important;
}

.ql-snow .ql-tooltip a.ql-remove::before {
  content: var(--ql-remove) !important;
}

.ql-snow .ql-tooltip[data-mode=video]::before {
  content: var(--ql-enterVideo) !important;
}

.ql-editor {
  font-size: 14px;
}

.ql-editor p,
.ql-editor h1,
.ql-editor h2,
.ql-editor h3,
.ql-editor h4,
.ql-editor h5,
.ql-editor h6,
.ql-editor div 
{
  margin-bottom: 4px !important;
}

button.ql-image2 {
  background: initial !important;
  background-image: url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABIAAAASCAYAAABWzo5XAAAACXBIWXMAAAsTAAALEwEAmpwYAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAABhSURBVHgB7ZJBDsAgCATB9N/2p30KvdAGEZS9OwkH47jZqESHHezWQhicbSBBg9sS6dEp0wC365SQRYjo9MiNLpvN4Y/beezctJFtEI11/+So0e71pkbXQoSIgtBPeSjyAqF4GzqalpvTAAAAAElFTkSuQmCC) !important;
  background-size: contain !important;
}

.ql-editor h1 { font-size: 40px !important; color: rgb(237, 28, 41); }
.ql-editor h2 { font-size: 36px !important; font-weight: 500 !important; color: rgb(237, 28, 41); }
.ql-editor h3 { font-size: 36px !important; font-weight: 300 !important; color: rgb(237, 28, 41); }
.ql-editor h4 { font-size: 32px !important; font-weight: 500 !important; color: rgb(237, 28, 41); }
.ql-editor h5 { font-size: 24px !important; font-weight: 500 !important; color: rgb(237, 28, 41); }
.ql-editor h6 { font-size: 18px !important; font-weight: 500 !important; color: rgb(237, 28, 41); }

.ql-editor h1:not(:first-child) { margin-top: 18px !important; }
.ql-editor h2:not(:first-child) { margin-top: 18px !important; }
.ql-editor h3:not(:first-child) { margin-top: 18px !important; }
.ql-editor h4:not(:first-child) { margin-top: 18px !important; }
.ql-editor h5:not(:first-child) { margin-top: 18px !important; }
.ql-editor h6:not(:first-child) { margin-top: 18px !important; }

.ql-editor a {
  color: rgb(237, 28, 41) !important;
}

.ql-editor .ql-indent-1 > .ql-ui::before {
  content: "" !important;
  display: none !important;
}
.ql-editor p::after {
  content: "";
  display: table;
  clear: both;
}
</style>