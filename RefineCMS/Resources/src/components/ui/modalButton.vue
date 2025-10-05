<template>
    <div>
        <a-button type="primary" @click="open = true">{{ label }}</a-button>
        <a-modal v-model:open="open" :title="label" :width="`${width}px`" :closable="false" :destroy-on-close="true" :mask-closable="false" :ok-button-props="{ disabled }" @ok="confirmOK">
            <slot></slot>
        </a-modal>
    </div>
</template>

<script>
export default {
  props: {
    label: String,
    width: { type: Number, default: 1000 },
    disabled: { type: Boolean, default: false }
  },
  data() {
    return {
        open: false,
    }
  },
  methods: {
    confirmOK(e) {
      this.$emit('ok', e);
    }
  }
}
</script>