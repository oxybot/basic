import { IconLoader, IconChevronUp, IconChevronDown } from "@tabler/icons";
import { createColumnHelper, flexRender, getCoreRowModel, useReactTable } from "@tanstack/react-table";
import clsx from "clsx";
import { useMemo } from "react";
import { useNavigate } from "react-router-dom";
import EntityFieldView from "./EntityFieldView";

const columnHelper = createColumnHelper();

export default function EntityList({
  loading,
  definition,
  elements,
  baseTo = null,
  selectedId,
  simple = false,
  sorting,
  setSorting,
}) {
  const columns = useMemo(() => {
    if (!definition || !definition.fields) {
      return [];
    }

    return definition.fields
      .filter((i) => i.type !== "key")
      .map((field) =>
        columnHelper.accessor(field.name, {
          header: field.displayName,
          cell: (info) => <EntityFieldView type={field.type} value={info.getValue()} list />,
          enableSorting: field.sortable,
        })
      );
  }, [definition]);

  const navigate = useNavigate();

  const table = useReactTable({
    data: elements,
    columns,
    state: {
      sorting,
    },
    enableSorting: !simple,
    sortDescFirst: false,
    onSortingChange: setSorting,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <div className="table-responsive">
      <table className="table card-table table-vcenter text-nowrap datatable table-hover">
        <thead>
          {table.getHeaderGroups().map((headerGroup) => (
            <tr key={headerGroup.id}>
              {headerGroup.headers.map((header) => (
                <th key={header.id}>
                  {!header.isPlaceholder && header.column.getCanSort() && (
                    <button
                      class={clsx("table-sort", header.column.getIsSorted())}
                      onClick={header.column.getToggleSortingHandler()}
                    >
                      {flexRender(header.column.columnDef.header, header.getContext())}
                    </button>
                  )}
                  {!header.isPlaceholder && !header.column.getCanSort() && (
                    <>{flexRender(header.column.columnDef.header, header.getContext())}</>
                  )}
                </th>
              ))}
            </tr>
          ))}
        </thead>
        <tbody>
          <tr className={loading ? "" : "d-none"}>
            <td colSpan={table.getHeaderGroups().length}>
              <IconLoader /> Loading...
            </td>
          </tr>
          {table.getRowModel().rows.map((row) => (
            <tr
              key={row.id}
              className={clsx({
                "table-active": row.original.identifier === selectedId,
              })}
              onClick={() => baseTo !== null && navigate([baseTo, row.original.identifier].filter((i) => i).join("/"))}
            >
              {row.getVisibleCells().map((cell) => (
                <td key={cell.id} className={cell.column.id}>
                  {flexRender(cell.column.columnDef.cell, cell.getContext())}
                </td>
              ))}
            </tr>
          ))}
          {!loading && elements.length === 0 && (
            <tr>
              <td colSpan={table.getHeaderGroups().length}>
                <em>No results</em>
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}
