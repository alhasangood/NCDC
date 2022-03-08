import { extend, localize, setInteractionMode } from "vee-validate";
import { required,required_if, alpha_spaces, alpha_num, email, min, max, between,numeric, size, min_value, max_value,length } from "vee-validate/dist/rules";
import ar from "vee-validate/dist/locale/ar.json";

setInteractionMode("eager");

extend("required", required);
extend("required_if", required_if);
extend("numeric", numeric);
extend("alpha_num", alpha_num);
extend("alpha_spaces", alpha_spaces);
extend("email", email);
extend("min", min);
extend("max", max);
extend("length", length);
extend("between", between);
extend("size", size);
extend("min_value", min_value);
extend("max_value", max_value);


extend("captcha", { validate: value => /^[a-zA-Z0-9]{5}$/.test(String(value)) });

extend("alpha_num_spaces", {
  validate: value => /^(?!\s)[\u0621-\u064Aa-zA-Z0-9 ]+$/.test(String(value))
});

extend("arabic", {
  validate: value => /^(?!\s)[\u0621-\u064A ]+$/.test(String(value))
});

extend("english", { validate: value => /^[a-zA-Z ]+$/.test(String(value)) });

extend("english_num", { validate: value => /^[a-zA-Z0-9]+$/.test(String(value)) });

extend("decimal", { validate: value => /^-?[0-9]+(.[0-9])?$/.test(String(value)) });

extend("username", {
  validate: value => /^[a-zA-Z0-9._-]+$/.test(String(value))
});

extend("phone", { validate: value => /^[0-9]{9,}$/.test(String(value)) });

extend("password", {
  validate: value =>
    /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/g.test(
      String(value)
    )
});

extend("confirmPassword", {
  params: ["target"],
  validate: (value, { target }) => value === target
});

extend("olderThan", {
  params: ["target"],
  validate: (value, { target }) => value >= target
});

extend("studyYearOrder", {
  params: ["target"],
  validate: (value, { target }) => {
    if(!value || !target) {
      return true
    }
    return (parseInt(value.slice(value.length - 4)) < parseInt(target.slice(target.length - 4)))
  }
});

extend("confirmation", {
  validate: value => !!value
});

extend("libyanPhone", {
  validate: (value) => {
      let operatorCodes = ["92", "94", "91", "93", "95", "96"];
      let operatorCodesWithZero = ["092", "094", "091", "093", "095", "096"];
      let check = [];
      if (!value || !/^[0-9+]+$/.test(value)) return false;
      else {
          switch (value.length) {
              case 9:
                  check = operatorCodes.filter(x => x == value.substring(0, 2));
                  break;
              case 10:
                  check = operatorCodesWithZero.filter(x => x == value.substring(0, 3));
                  break;
              default:
                  break;
          }
          return check.length;
      }
  }
});
extend("longitude", {
    validate: value => parseInt(value) <= 180 && parseInt(value) >= -180 && /^[0-9+-]+$/.test(String(value))
});
extend("latitude", {
    validate: value => parseInt(value) <= 90 && parseInt(value) >= -90 && /^[0-9+-]+$/.test(String(value))
});
extend("nationalNumber", {
  validate: value => {
    const length = value.length;
    if (length !== 12) return false;
    if (value.substring(0, 1) !== "1" && value.substring(0, 1) !== "2")
      return false;
    if (parseInt(value.substring(1, 5)) <= 1800) return false;

    const checksum = parseInt(value.substring(11, 12));
    let weightSum = 0;
    let calculatedChecksum = 0;
    let i;

    for (i = 0; i < length - 1; i++) {
      switch (i) {
        case 0:
        case 3:
        case 6:
        case 9:
          weightSum += parseInt(value.substring(i, i + 1)) * 1;
          break;
        case 1:
        case 4:
        case 7:
        case 10:
          weightSum += parseInt(value.substring(i, i + 1)) * 3;
          break;
        case 2:
        case 5:
        case 8:
          weightSum += parseInt(value.substring(i, i + 1)) * 7;
          break;
      }
    }
    calculatedChecksum = (10 - (weightSum % 10)) % 10;
    if (calculatedChecksum !== checksum) return false;
    return true;
  }
});


localize("ar", ar);

localize({
  ar: {
    messages: {
      required: "هذا الحقل مطلوب",
      captcha: "{_field_} الذي ادخلته غير مطابق لرمز التحقق",
      required_if: "هذا الحقل مطلوب",
      alpha_num_spaces:"يجب أن يحتوي {_field_} على أحرف عربية، انجليزية، مسافات و أرقام فقط",
      english_num: 
      "يجب أن يحتوي {_field_} على أحرف انجليزية و أرقام فقط",
      alpha_spaces: 
      "يجب أن يحتوي {_field_} على أحرف عربية أو انجليزية ومسافات فقط",
      email: "يجب ادخال بريد الكتروني صحيح",
      decimal: "يجب أن تكون {_field_} عدد صحيح أو كسر عشري",
      min: (field, { length }) => `يجب أن يكون طول ${field}  ${length} أو أكثر`,
      max: (field, { length }) => `يجب أن يكون طول ${field}  ${length} أو أقل`,
      length: (field, { length }) => `يجب أن يكون طول ${field}  ${length}`,
      size: (field, { size }) => `يجب أن يكون حجم ${field}  ${size} كيلوبايت أو أقل`,
      between: (field, { min, max }) =>
        `يجب أن تكون قيمة ${field} بين ${min} و ${max}`,
      min_value: (field, { min }) => `يجب أن تكون قيمة  ${field}   ${min} أكثر من او تساوي `,
      max_value: (field,  {max }) => `يجب أن تكون قيمة  ${field}  ${max}  أقل من او تساوي`,
      olderThan: (field, { target }) => `يجب أن تكون ${field} بعد  ${target}`,
      arabic: "هذا الحقل يجب أن يحتوي على أحرف عربية ومسافات فقط",
      english: "هذا الحقل يجب أن يحتوي على أحرف انجليزية ومسافات فقط",
      username: "اسم المستخدم غير صالح",
      phone: "يجب ادخال رقم هاتف صحيح",
      password: 
        "يجب أن تحتوي {_field_} على رقم, أحرف كبيرة وصغيرة، رمز (@$!%*?&)",
      confirmPassword: "كلمات المرور غير متطابقة",
      nationalNumber: "الرقم الوطني غير صحيح",
      numeric: "الرجاء ادخال الحقل بصيغة ارقام فقط",
      libyanPhone: "يجب ادخال رقم هاتف جوال ليبي",
      confirmation: "الرجاء الإقرار",
      studyYearOrder: (field, { target }) => `يجب أن يكون ${field} أقدم من ${target}`,
          longitude: "قيمة خط الطول تكون بين -180 و 180",
          latitude: "قيمة خط العرض تكون بين -90 و 90",
      }

  }
});
