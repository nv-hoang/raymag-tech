<template>
  <div>
    <a-popconfirm :title="$t('deleteConfirm.title')" :ok-text="$t('deleteConfirm.yes')" :cancel-text="$t('deleteConfirm.no')" @confirm="$refs.deleteButton.click()">
      <a-button :type="type" danger>
        <slot></slot>
      </a-button>
    </a-popconfirm>
    <button ref="deleteButton" type="submit" name="SubmitAction" :value="name" style="display: none"></button>
  </div>
</template>

<script>
export default {
  props: {
    type: { type: String, default: 'link' },
    name: { type: String, default: 'delete' },
  },
}
</script>