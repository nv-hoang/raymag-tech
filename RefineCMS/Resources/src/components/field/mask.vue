<template>
  <a-form-item :help="getMessage()" :validate-status="getMessage() ? 'error':''">
    <input :disabled="disabled" type="text" class="ant-input" :value="fvalue" v-mask="mask" @change="(e) => change(e.target.value)" :placeholder="placeholder" />
  </a-form-item>
</template>

<script>
import base from './base';
export default {
  mixins: [base],
  props: {
    mask: { type: String, required: true }
  },
}
</script>