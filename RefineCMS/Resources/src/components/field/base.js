export default {
  props: {
    name: { type: String, required: true },
    label: String,
    desc: String,
    placeholder: String,
    fvalue: { default: null },
    fdefault: { default: null },
    showif: { type: Boolean, default: true },
    expand: { type: String, default: 'true' },
    required: { type: Boolean, default: false },
    disabled: { type: Boolean, default: false },
    message: String
  },
  data() {
    return {
      valid: false,
      msg: this.message,
    };
  },
  methods: {
    change(val) {
      if (val != this.fvalue) {
        this.$emit('field-change', val);
        this.msg = '';
        this.valid = true;
      }
    },
    isEmpty() {
      return this.fvalue == '';
    },
    getMessage() {
      if(!this.showif) {
        this.msg = '';
        return '';
      }
      return this.msg ? this.msg : (this.valid && this.required && (this.fvalue === null || this.isEmpty()) ? this.$t('inputRequired') : '');
    },
    checkValid(e) {
      this.valid = true;
      if (this.getMessage()) {
        e.preventDefault();
      }
    }
  },
  mounted() {
    this.$nextTick(() => {
      this.$emit('field-mounted', this);
    });
  }
}