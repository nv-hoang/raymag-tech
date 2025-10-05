import { h, reactive, onMounted, getCurrentInstance } from 'vue'

export default {
  props: {
    data: { type: Object, default: {} },
  },
  setup(props, { attrs, slots, emit }) {
    const state = reactive(props.data);
    const instance = getCurrentInstance();
    const emitter = instance.appContext.config.globalProperties.emitter;

    onMounted(() => {
      for (const attr in attrs) {
        if (attr.startsWith('on')) {
          var eventName = attr.replace('on', '').replace(/([a-z])([A-Z])/g, '$1-$2').toLowerCase();
          emitter.on(eventName, (e) => emit(eventName, state, e))
        }
      }
    });

    return () => h('div', { class: attrs.class, style: attrs.style }, slots.default ? slots.default(state) : '');
  },
}