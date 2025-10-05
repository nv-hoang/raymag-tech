<template>
  <a-form-item :help="getMessage()" :validate-status="getMessage() ? 'error':''">
    <a-input :value="fvalue" @change="(e) => change(e.target.value)" :placeholder="placeholder" :disabled="disabled"></a-input>
  </a-form-item>
</template>

<script>
import base from './base';
export default {
  mixins: [base],
}
</script>