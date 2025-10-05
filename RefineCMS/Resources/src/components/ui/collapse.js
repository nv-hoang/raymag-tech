import { h, ref, onMounted, watch, nextTick } from 'vue'

export default {
  props: {
    open: { type: Boolean, default: true },
  },
  setup(props, { slots }) {
    const containerRef = ref(null);
    const maxHeight = ref(props.open ? '' : '0px');

    const updateHeight = async () => {
      await nextTick()
      if (containerRef.value) {
        maxHeight.value = props.open ? `${containerRef.value.scrollHeight}px` : '0px';
      }
    };

    onMounted(updateHeight);
    watch(() => props.open, updateHeight);

    return () => h('div', {
      ref: containerRef,
      class: 'relative overflow-hidden transition-all duration-300',
      style: `max-height: ${maxHeight.value}`
    }, slots.default ? slots.default() : '');
  },
}
