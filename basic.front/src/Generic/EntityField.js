import { IconChevronRight } from "@tabler/icons";
import dayjs from "dayjs";
import { Link } from "react-router-dom";

export default function EntityField({ type, value }) {
  if (value === undefined || value === null) {
    return "-";
  }

  switch (type) {
    case "datetime":
      return dayjs(value).format("DD MMM YYYY hh:mm:ss");

    case "date":
      return dayjs(value).format("DD MMM YYYY");

    case "ref/client":
      return (
        <div className="d-flex align-items-start">
          {value.displayName}
          <Link
            className="ms-auto btn btn-sm btn-outline-secondary"
            to={`/client/${value.identifier}`}
          >
            <IconChevronRight /> See details
          </Link>
        </div>
      );

    case "string":
      return value;

    default:
      console.warn("unknown field type: " + type + " - rendered as string");
      return value;
  }
}
