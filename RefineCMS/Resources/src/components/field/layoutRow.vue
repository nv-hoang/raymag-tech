<template>
  <div v-show="showif" class="field-layout-row flex flex-col md:flex-row" :class="label ? 'border border-gray-200':''">
    <div v-if="label" class="md:w-[120px] p-2 pb-0 md:py-2 md:px-3 md:bg-gray-50">
      <div class="font-medium">{{ label }} <sup v-if="required" class="text-red-500 text-[14px] -top-1">*</sup></div>
      <div v-if="desc" class="text-[12px]" v-html="desc"></div>
    </div>
    <div :class="label ? 'flex-1 p-2 md:border-l border-gray-200':'flex-1'">
      <slot></slot>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    label: String,
    desc: String,
    showif: { type: Boolean, default: true },
    required: { type: Boolean, default: false },
  },
}
</script>