<template>
  <a-form-item :help="getMessage()" :validate-status="getMessage() ? 'error':''">
    <a-radio-group :value="fvalue" @change="(e) => change(e.target.value)" :placeholder="placeholder" :disabled="disabled">
      <slot></slot>
    </a-radio-group>
  </a-form-item>
</template>

<script>
import base from './base';
export default {
  mixins: [base],
}
</script>