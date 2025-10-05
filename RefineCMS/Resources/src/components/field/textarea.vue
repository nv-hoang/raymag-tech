<template>
  <a-form-item :help="getMessage()" :validate-status="getMessage() ? 'error':''">
    <a-textarea :value="fvalue" @change="(e) => change(e.target.value)" :placeholder="placeholder" :disabled="disabled" :rows="rows"></a-textarea>
  </a-form-item>
</template>

<script>
import base from './base';
export default {
  mixins: [base],
  props: {
    rows: { type: Number, default: 3 },
  },
}
</script>