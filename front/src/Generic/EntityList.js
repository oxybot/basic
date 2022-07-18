import { IconLoader } from "@tabler/icons";
import clsx from "clsx";
import { useNavigate } from "react-router-dom";
import EntityFieldView from "./EntityFieldView";

function filtered(fields) {
  if (!fields) {
    return fields;
  }

  return fields.filter((i) => i.type !== "key");
}

export default function EntityList({ loading, definition, entities, baseTo = null, selectedId, filter }) {
  const navigate = useNavigate();
  const fields = filtered(definition?.fields);

  const regex = new RegExp(`.{1,${filter}}`, "g");
  console.log("1 " + regex);

  var replace = "regex\\d";
  var re = new RegExp(replace,"g");
  console.log("2 " + re);

  var essai = "mystring1".replace(re, "newstring");

  console.log("3 " + replace);
  console.log("4 " + essai);
  console.log("----------------------");


  return (
    <div className="table-responsive">
      <table className="table card-table table-vcenter text-nowrap datatable table-hover">
        <thead>
          <tr>
            {fields &&
              fields.map((field, index) => (
                <th key={index} className={clsx({ "w-1": index === 0 })}>
                  {field.displayName}
                </th>
              ))}
          </tr>
        </thead>
        <tbody>
          <tr className={loading ? "" : "d-none"}>
            <td colSpan={fields?.length}>
              <IconLoader /> Loading...
            </td>
          </tr>
          {/* {entities.filter(e => Object.values(e).includes(filter)).map((entity) => ( */}
          {entities.filter(e => Object.values(e).includes(/{filter}/g)).map((entity) => (
          // {entities.map((entity) => (
            <tr
              key={entity.identifier}
              className={clsx({
                "table-active": entity.identifier === selectedId,
              })}
              onClick={() => baseTo !== null && navigate([baseTo, entity.identifier].filter((i) => i).join("/"))}
            >
              {entity &&
                fields &&
                fields.map((field, index) => (
                  <td key={index}>
                    <EntityFieldView type={field.type} value={entity[field.name]} list />
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