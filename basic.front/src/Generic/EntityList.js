import { IconLoader } from "@tabler/icons";
import clsx from "clsx";
import { useNavigate } from "react-router-dom";
import EntityField from "./EntityField";

function filtered(fields) {
  if (!fields) {
    return fields;
  }

  return fields.filter((i) => i.type !== "key");
}

export default function EntityList({
  loading,
  definition,
  entities,
  baseTo,
  selectedId,
}) {
  const navigate = useNavigate();
  const fields = filtered(definition?.fields);

  return (
    <div className="table-responsive">
      <table className="table card-table table-vcenter text-nowrap datatable table-hover">
        <thead>
          <tr>
            {fields &&
              fields.map((field, index) => (
                <th key={index}>{field.displayName}</th>
              ))}
          </tr>
        </thead>
        <tbody>
          <tr className={loading ? "" : "d-none"}>
            <td colSpan={fields?.length}>
              <IconLoader /> Loading...
            </td>
          </tr>
          {entities.map((entity) => (
            <tr
              key={entity.identifier}
              className={clsx({
                "table-active": entity.identifier === selectedId,
              })}
              onClick={() =>
                navigate([baseTo, entity.identifier].filter((i) => i).join("/"))
              }
            >
              {entity &&
                fields &&
                fields.map((field, index) => (
                  <td key={index}>
                    <EntityField
                      type={field.type}
                      value={entity[field.name]}
                      list
                    />
                  </td>
                ))}
            </tr>
          ))}
          {!loading && entities.length === 0 && (
            <tr>
              <td colSpan={fields?.length}>
                <em>No results</em>
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}
