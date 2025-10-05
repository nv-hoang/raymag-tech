<template>
  <div style="max-width: 500px;">
    <ui-nested :items="items" :edit-link="editLink"></ui-nested>

    <textarea name="FormData" :value="JSON.stringify(items)" style="display: none;"></textarea>
  </div>
</template>

<script>
export default {
  props: {
    editLink: String
  },
  data() {
    var items = [];
    if(this.$slots.default) {
      var el = this.$slots.default()[0];
      if(el.type == 'textarea' && el.children) {
        items = JSON.parse(el.children);
      }
    }

    return {
      items: items,
    }
  },
}
</script>