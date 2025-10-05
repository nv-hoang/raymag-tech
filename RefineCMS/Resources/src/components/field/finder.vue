<template>
  <div>
    <a-modal :open="open" @update:open="val => $emit('update:open', val)" width="90%" centered :closable="false" :destroy-on-close="true" :mask-closable="false" :ok-button-props="{ disabled: selection == null }" @ok="confirmSelect">
        <vue-finder id="vf" :request="request" :features="features" :max-file-size="$root.MaxUploadSize" path="local://" persist @select="handleSelect"></vue-finder>
    </a-modal>
  </div>
</template>

<script>
import {FEATURES} from "vuefinder/dist/features.js";

export default {
  props: {
      api: { type: String, default: '/admin/storage' },
      imageOnly: { type: Boolean, default: false },
      open: { type: Boolean, default: false },
  },
  data() {
    return {
      request: {
        baseUrl: this.api,
        transformRequest: req => {
          if(['newfolder', 'upload', 'delete', 'move', 'rename'].includes(req.params.q)) {
            req.url = `${req.url}/${req.params.q}`;
          }
          return req;
        },
      },
      features: [
        FEATURES.NEW_FOLDER,
        FEATURES.PREVIEW,
        FEATURES.SEARCH,
        FEATURES.RENAME,
        FEATURES.UPLOAD,
        FEATURES.DELETE,
        FEATURES.DOWNLOAD,
      ],
      selection: null,
    }
  },
  methods: {
    handleSelect(items) {
      if(items && items.length != undefined && items.length == 1 && items[0].url != undefined && (!this.imageOnly || this.isImage(items[0].url))) {
        this.selection = items[0].url;
      } else {
        this.selection = null;
      }
    },
    confirmSelect() {
      if(this.selection) {
        if(!this.imageOnly || this.isImage(this.selection)) {
            this.$emit('update:open', false);
            this.$emit('selected', this.selection);
        }
      }
    },
    isImage(url) {
      return /\.(jpg|jpeg|png|gif|bmp|webp|svg|ico|avif)(\?.*)?$/i.test(url);
    },
    getFileName(url) {
      return url.split('/').pop().split('?')[0].split('#')[0];
    }
  }
}
</script>

<style>
.uploadfile-preview:hover .delicon {
  display: flex !important;
}
</style>