<template>
  <a-form-item :help="getMessage()" :validate-status="getMessage() ? 'error':''">
    <a-select show-search :disabled="disabled" :mode="multiple ? 'multiple' : ''" :value="fvalue || (multiple ? [] : '')" @change="(e) => change(e)" :placeholder="placeholder" option-filter-prop="alt" :filter-option="filterOption" class="w-full">
      <slot></slot>
    </a-select>
  </a-form-item>
</template>

<script>
import base from './base';
export default {
  mixins: [base],
  props: {
    multiple: { type: Boolean, default: false }
  },
  methods: {
    filterOption(input, option) {
      if(option.alt) {
        return option.alt.toLowerCase().includes(input.toLowerCase());;
      }
      return false;
    },
  }
}
</script>