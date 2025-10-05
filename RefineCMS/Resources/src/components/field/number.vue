<template>
  <a-form-item :help="getMessage()" :validate-status="getMessage() ? 'error':''">
    <a-input-number :disabled="disabled" :value="fvalue" :min="min" :max="max" @change="(v) => change(v)"></a-input-number>
  </a-form-item>
</template>

<script>
import base from './base';
export default {
  mixins: [base],
  props: {
    min: { type: Number, default: -Infinity },
    max: { type: Number, default: Infinity },
  }
}
</script>