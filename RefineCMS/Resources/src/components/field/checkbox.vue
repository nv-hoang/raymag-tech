<template>
  <a-form-item :help="getMessage()" :validate-status="getMessage() ? 'error':''">
    <a-checkbox-group :value="fvalue" @change="(e) => change(e)" :placeholder="placeholder" :disabled="disabled">
      <slot></slot>
    </a-checkbox-group>
  </a-form-item>
</template>

<script>
import base from './base';
export default {
  mixins: [base],
}
</script>