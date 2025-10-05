<template>
  <a-form-item :help="getMessage()" :validate-status="getMessage() ? 'error':''">
    <a-slider :disabled="disabled" :value="fvalue" :min="min" :max="max" :tip-formatter="(v) => tipFormatter.replace('{0}', v || '')" @change="(v) => change(v)"></a-slider>
  </a-form-item>
</template>

<script>
import base from './base';
export default {
  mixins: [base],
  props: {
    min: { type: Number, default: 0 },
    max: { type: Number, default: 100 },
    tipFormatter: { type: String, default: '{0}' }
  }
}
</script>