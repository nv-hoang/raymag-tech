<template>
  <div>
    <input type="hidden" :name="name" :value="fvalue">
  </div>
</template>

<script>
import base from './base';
export default {
  mixins: [base],
}
</script>