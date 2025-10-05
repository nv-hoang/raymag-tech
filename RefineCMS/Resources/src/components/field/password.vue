<template>
  <a-form-item :help="getMessage()" :validate-status="getMessage() ? 'error':''">
    <a-space>
      <a-input-password :disabled="disabled" :value="fvalue" @change="(e) => change(e.target.value)" :placeholder="placeholder" autocomplete="current-password"></a-input-password>
      <a-button v-if="showButton" :disabled="disabled" @click="change(generate())">{{ $t('generate') }}</a-button>
    </a-space>
  </a-form-item>
</template>

<script>
import base from './base';
export default {
  mixins: [base],
  props: {
    showButton: { type: Boolean, default: false }
  },
  methods: {
    generate(length = 12) {
      const chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+[]{}|;:,.<>?";
      let password = "";
      for (let i = 0; i < length; i++) {
        password += chars.charAt(Math.floor(Math.random() * chars.length));
      }
      return password;
    }
  }
}
</script>