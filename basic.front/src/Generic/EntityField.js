import { IconChevronRight } from "@tabler/icons";
import clsx from "clsx";
import dayjs from "dayjs";
import pluralize from "pluralize";
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

    case "number/hours":
      return pluralize("hour", value, true);

    case "ref/category":
      return value.displayName;

    case "ref/client":
    case "ref/user":
      if (list) {
        return value.displayName;
      } else {
        const linkTo = type.replace("ref/", "");
        return (
          <div className="d-flex align-items-start">
            {value.displayName}
            <Link className="ms-auto btn btn-sm btn-outline-secondary" to={`/${linkTo}/${value.identifier}`}>
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

    case "schedule":
      if (!value) {
        return value;
      } else if (list) {
        if (value.length <= 7) {
          return pluralize(
            "hour",
            value.reduce((a, c) => a + c),
            true
          );
        } else {
          const odd = value.slice(0, 7).reduce((a, c) => a + c);
          const even = value.slice(7).reduce((a, c) => a + c);
          return `${odd} / ${even} hours`;
        }
      } else {
        const days = [0, 1, 2, 3, 4, 5, 6];
        const complex = value.length > 7;
        return (
          <table className="table mt-1 table-sm text-center">
            <thead>
              <tr>
                {complex && <th className="m-0 p-0 w-20"></th>}
                {days.map((d) => {
                  const dayShort = dayjs().day(d).format("ddd");
                  return (
                    <th key={d} className="m-0 p-0 w-10">
                      {dayShort}
                    </th>
                  );
                })}
              </tr>
            </thead>
            <tbody>
              <tr>
                {complex && <td className="text-nowrap">Odd weeks</td>}
                {days.map((d) => (
                  <td key={d} className="w-10">
                    {value[d]}
                  </td>
                ))}
              </tr>
              {complex && (
                <tr>
                  <td className="text-nowrap">Even weeks</td>
                  {days.map((d) => (
                    <td key={d} className="w-10">
                      {value[d + 7]}
                    </td>
                  ))}
                </tr>
              )}
            </tbody>
          </table>
        );
      }

    case "color":
      return <div className={clsx(value, "icon rounded")}></div>;

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
