import { h, defineComponent } from 'vue';

export default {
  props: {
    name: String,
    title: String,
    width: { type: Number, default: 0 },
    row: { type: Object, default: {} },
  },
  setup(props, { slots }) {
    const InlineComponent = defineComponent({
      template: '<div>'+ (props.row[props.name] || '') +'</div>'
    });
    return () => (slots.default ? slots.default(props.row) : h(InlineComponent));
  },
}