<template>
  <a-form-item :help="getMessage()" :validate-status="getMessage() ? 'error':''">
    <div :style="disabled ? 'pointer-events: none; opacity: 0.5;':''">
      <div v-if="fvalue" style="padding-right: 16px;">
        <div class="inline-block relative mb-2 uploadfile-preview">
          <div v-if="isImage(fvalue)">
            <img :src="fvalue" style="max-width: 100%;">
          </div>
          <a v-else :href="fvalue" target="_blank" class="flex gap-1 border border-gray-200 rounded-[6px] px-2 py-1 pr-5">
            <FileOutlined></FileOutlined>
            <div class="truncate inline-block" style="max-width: 200px;">{{ getFileName(fvalue) }}</div>
          </a>
      
          <div @click="change('')" style="display: none;" class="delicon absolute -top-2 -right-3 size-[24px] rounded-full border border-gray-200 bg-white items-center justify-center cursor-pointer text-[12px] text-gray-400 hover:text-primary-500 hover:border-primary-500">
            <CloseOutlined></CloseOutlined>
          </div>
        </div>
      </div>

      <a-button size="small" @click="open = true">{{ $t('choose_file') }}</a-button>

      <Finder v-model:open="open" :api="api" :image-only="imageOnly" @selected="(url) => change(url)"></Finder>
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
    api: { type: String, default: '/admin/storage' },
    imageOnly: Boolean,
  },
  data() {
    return {
      open: false,
    }
  },
  methods: {
    isImage(url) {
      return /\.(jpg|jpeg|png|gif|bmp|webp|svg|ico|avif)(\?.*)?$/i.test(url);
    },
    getFileName(url) {
      return url.split('/').pop().split('?')[0].split('#')[0];
    }
  }
}
</script>