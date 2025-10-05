<template>
  <a-form-item :help="getMessage()" :validate-status="getMessage() ? 'error':''">
    <slot></slot>
  </a-form-item>
</template>

<script>
import base from './base';
export default {
  mixins: [base],
}
</script>