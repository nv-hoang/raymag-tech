<template>
  <a-form-item :help="getMessage()" :validate-status="getMessage() ? 'error':''">
    <a-select :disabled="disabled" mode="tags" :value="fvalue || []" @change="(e) => change(e)" :placeholder="placeholder" class="w-full">
      <slot></slot>
    </a-select>
  </a-form-item>
</template>

<script>
import base from './base';
export default {
  mixins: [base],
}
</script>