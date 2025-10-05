<template>
  <a-form-item :help="getMessage()" :validate-status="getMessage() ? 'error':''">
    <a-date-picker :disabled="disabled" :value="fvalue ? dayjs(fvalue) : null" @change="(date, dateString) => change(dateString)" :showTime="time" :picker="picker" :locale="$tm('datePicker')"></a-date-picker>
  </a-form-item>
</template>

<script>
import base from './base';
export default {
  mixins: [base],
  props: {
    time: { type: Boolean, default: false },
    picker: { type: String, default: 'date' },
  }
}
</script>
