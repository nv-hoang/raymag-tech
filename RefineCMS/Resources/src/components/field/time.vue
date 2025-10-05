<template>
  <a-form-item :help="getMessage()" :validate-status="getMessage() ? 'error':''">
    <a-time-picker :disabled="disabled" :value="fvalue ? dayjs(fvalue, 'HH:mm') : null" format="HH:mm" :locale="$tm('datePicker')" @change="(time, timeString) => change(timeString)"></a-time-picker>
  </a-form-item>
</template>

<script>
import base from './base';
export default {
  mixins: [base],
}
</script>
