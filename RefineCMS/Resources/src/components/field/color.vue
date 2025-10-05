<template>
  <a-form-item :help="getMessage()" :validate-status="getMessage() ? 'error':''">
    <div class="inline-block p-[3px] border border-gray-200 rounded-[6px]" :style="disabled ? 'pointer-events: none; opacity: 0.5;':''">
      <div ref="colorPicker"></div>
    </div>
  </a-form-item>
</template>

<script>
import Pickr from '@simonwep/pickr';
import '@simonwep/pickr/dist/themes/nano.min.css';
import base from './base';
export default {
  mixins: [base],
  data() {
    return { colorPicker: null }
  },
  watch: {
    fvalue(val, oldVal) {
      this.colorPicker.setColor(val);
    },
  },
  mounted() {
    this.colorPicker = Pickr.create({
      el: this.$refs.colorPicker,
      theme: 'nano',
      default: this.fvalue,
      components: {
        // Main components
        preview: true,
        opacity: true,
        hue: true,

        // Input / output Options
        interaction: {
          input: true,
          clear: true,
          save: true
        }
      },
      i18n: {
        'btn:save': this.$t('save'),
        'btn:cancel': this.$t('cancel'),
        'btn:clear': this.$t('clear'),
      }
    });

    this.colorPicker.on('save', (color, instance) => {
      this.change(color ? color.toHEXA().toString() : '');
      this.colorPicker.hide();
    });
  }
}
</script>

<style>
.pickr .pcr-button {
  width: 24px !important;
  height: 24px !important;
  box-shadow: none !important;
}
</style>