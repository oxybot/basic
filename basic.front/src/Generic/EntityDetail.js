import { IconChevronRight } from "@tabler/icons";
import dayjs from "dayjs";
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { getDefinition } from "../api";
import { groupBy, objectMap } from "../helpers";

function Field({ type, value }) {
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
            className="ms-auto btn btn-sm btn-outline-primary"
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

export default function EntityDetail({ definitionName, entity }) {
  const [definition, setDefinition] = useState(null);

  useEffect(() => {
    getDefinition(definitionName)
      .then((definition) => setDefinition(definition))
      .catch((err) => console.log(err));
  }, [definitionName]);

  return (
    <>
      {definition &&
        objectMap(
          groupBy(definition.fields, (x) => x.group),
          (fields, group, index) => (
            <div key={index} className="mb-3">
              <div className="card">
                {group !== "null" && (
                  <div className="card-header">
                    <h3 className="card-title">{group}</h3>
                  </div>
                )}
                <div className="card-body">
                  {fields.map((field, index) => (
                    <div key={index} className="mb-3">
                      <div className="field-label">{field.displayName}</div>
                      <p className="lead">
                        <Field
                          type={field.type}
                          value={entity[field.name] || "-"}
                        />
                      </p>
                    </div>
                  ))}
                </div>
              </div>
            </div>
          )
        )}
    </>
  );
}
