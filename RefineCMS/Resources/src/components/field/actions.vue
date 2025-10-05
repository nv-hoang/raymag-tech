<template>
  <div class="fixed bottom-0 left-0 w-full xl:pl-[250px] z-10">
    <div class="border-t border-gray-200 bg-white px-4 md:px-6 2xl:px-[48px] py-2">
      <div class="mx-auto" style="max-width: 1000px;">
        <div class="flex items-center gap-3">
          <a v-if="backUrl" :href="backUrl" class="flex items-center gap-2 text-[13px] text-gray-400 transition-colors hover:text-gray-800 focus:text-gray-800">
              <arrow-left-outlined class="text-[14px]"></arrow-left-outlined>
              <span class="hidden md:block">{{ $t('backToList') }}</span>
          </a>
          <div class="flex-1"></div>
          <slot></slot>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'form_actions',
  props: {
    backUrl: String
  },
}
</script>