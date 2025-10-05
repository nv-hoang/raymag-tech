<template>
  <a-form-item :help="getMessage()" :validate-status="getMessage() ? 'error':''">
    <a-switch :disabled="disabled" :checked="fvalue" checked-value="1" unChecked-value="0" @change="(e) => change(e)"></a-switch>
  </a-form-item>
</template>

<script>
import base from './base';
export default {
  mixins: [base],
}
</script>