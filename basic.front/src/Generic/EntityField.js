import { IconChevronRight } from "@tabler/icons";
import clsx from "clsx";
import dayjs from "dayjs";
import { Link } from "react-router-dom";
import { toCurrency } from "../helpers";

export default function EntityField({ type, value, list = false }) {
  if (value === undefined || value === null) {
    return "-";
  }

  switch (type) {
    case "datetime":
      return dayjs(value).format("DD MMM YYYY hh:mm:ss");

    case "date":
      return dayjs(value).format("DD MMM YYYY");

    case "ref/user":
    case "ref/category":
    case "ref/client":
      if (list) {
        return value.displayName;
      } else {
        return (
          <div className="d-flex align-items-start">
            {value.displayName}
            <Link className="ms-auto btn btn-sm btn-outline-secondary" to={`/client/${value.identifier}`}>
              <IconChevronRight /> See details
            </Link>
          </div>
        );
      }

    case "ref/eventtimemapping":
      switch (value) {
        case "Active":
          return value;
        case "StandardTimeOff":
          return "Standard Time-off";
        case "ExtraTimeOff":
          return "Extra Time-off";
        default:
          throw new Error("Unknown value: " + value);
      }

    case "image":
      if (value) {
        return (
          <img
            className={clsx("avatar", { "avatar-sm": list }, { "avatar-lg": !list })}
            alt=""
            src={`data:${value.mimeType};base64,${value.data}`}
          />
        );
      } else {
        return null;
      }

    case "boolean":
      return value ? "Yes" : "No";

    case "currency":
      return toCurrency(value);

    case "string":
      return value;

    default:
      console.warn("unknown field type: " + type + " - rendered as string");
      return value;
  }
}
