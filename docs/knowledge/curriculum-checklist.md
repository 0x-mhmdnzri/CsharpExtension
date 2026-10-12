# ماتریس پوشش C# و CLR

این فایل checklist اجباری برای طراحی rule، binder، runtime model و explanation است. وجود یک topic در این فهرست به‌معنای نمایش پیشنهاد همیشگی نیست؛ rule فقط وقتی فعال می‌شود که context، نسخه زبان، TFM، package و evidence آن را تأیید کنند.

## قرارداد پوشش

هر concept باید این metadata را داشته باشد:

```json
{
  "id": "clr.gc.generations",
  "areas": ["runtime", "memory", "performance"],
  "languageVersions": [6, 7, 8, 9, 10, 11, 12, 13, 14, 15],
  "tfm": ">=net6.0",
  "triggers": ["allocation", "finalizer", "large-object"],
  "evidenceRequired": true,
  "suggestionPolicy": "context-only"
}
```

## 1. اجرای CLR، assembly و deployment

- managed module، IL، metadata و managed execution model
- PE32/PE32+، headers، sections و native code
- manifest، multi-module assembly و resource
- CLR loading، x86/x64/ARM و WoW64
- JIT، native optimization، startup و working set
- safe/unsafe، type verification و PE validation
- NGen/ReadyToRun به‌عنوان runtime evidence
- CTS، CLS و interoperability با unmanaged/WinRT
- compiler، response file، CSC configuration و global usings
- ModuleDef، TypeDef، MethodDef، FieldDef، AssemblyRef، TypeRef و MemberRef
- version resource، AssemblyVersion، FileVersion و InformationalVersion
- culture و satellite assembly
- probing، binding redirect، publisher policy و type resolution
- weak/strong name، signing، delayed signing، GAC و tamper evidence

## 2. مدل انواع و dispatch

- Object و قراردادهای Equals، GetHashCode، ToString، GetType، Finalize
- allocation، zeroing، type object pointer و object identity
- cast، `is`، `as` و type safety
- namespace، assembly، using و alias
- stack، frame، local، managed heap و static dispatch
- virtual dispatch، method table و interface dispatch
- primitive/reference/value types
- checked/unchecked و overflow
- struct/class، layout، Sequential/Explicit/Auto و FieldOffset
- boxing/unboxing، interface boxing و هزینه allocation
- equality/identity و hash collection invariants
- dynamic، runtime binder، COM و reflection dispatch

## 3. انواع ضروری و امکانات زبان

- Unicode، string immutability، interning و StringComparison
- StringBuilder، formatting/parsing و IFormattable
- داده حساس و lifecycle حافظه رشته
- enum، underlying type، Flags و bitwise operations
- array، multidimensional، jagged، bounds check و covariance
- Span/Memory و safe high-performance access
- delegate، multicast invocation list، event و closure
- lambda و capture allocation
- custom attribute، AttributeUsage و metadata reflection
- Nullable<T>، boxing nullable، `??` و `?.`

## 4. exception، memory و resource lifetime

- exception flow، stack search، try/catch/finally و state management
- zero-cost normal path و هزینه throw
- InnerException، Data و custom exception design
- corrupted state و cleanup guarantees
- managed heap، Gen0/1/2، compaction و object lifetime
- GC.Collect، WaitForPendingFinalizers و KeepAlive
- Dispose، Finalize، SafeHandle و disposable pattern
- finalization queue و F-reachable queue
- LOH، pinning، fragmentation و memory pressure

## 5. hosting، loading، reflection و serialization

- CLR hosting و host boundary
- AppDomain، isolation، load/unload و cross-domain communication
- Assembly.Load/LoadFrom/LoadFile و AssemblyLoadContext
- reflection discovery، Activator و dynamic invocation
- Delegate.CreateDelegate، DynamicMethod و expression optimization
- metadata-only inspection
- serialization contract، Serializable/NonSerialized و lifecycle callbacks
- WinRT metadata و type marshaling

## 6. threading و asynchronous execution

- process/thread، worker thread، stack cost و context switch
- Thread و ThreadPool
- compute-bound، APM/EAP/TAP و Task/Task<T>
- AggregateException، CancellationToken و cooperative cancellation
- async/await و generated state machine
- blocking/non-blocking I/O و IOCP
- file/socket/HTTP asynchronous patterns
- atomicity، race، deadlock و memory ordering
- Volatile، Interlocked و spin constructs
- Mutex، Semaphore، events و kernel transition cost
- Monitor/lock، ReaderWriterLockSlim و SemaphoreSlim
- contention، fairness و hybrid synchronization

## استفاده در پیشنهادها

هر suggestion باید از این مراحل عبور کند:

1. context classifier، conceptهای مرتبط را انتخاب کند.
2. language version، TFM، package و configuration را اعمال کند.
3. symbol/bound tree و runtime model evidence تولید کنند.
4. rule فقط در صورت داشتن evidence کافی candidate بسازد.
5. speculative verifier خطا و nullable warning جدید را حذف کند.
6. explanation، topic tags و منابع را نمایش دهد.

پیشنهاد memory بدون allocation evidence، پیشنهاد API بدون metadata معتبر، و پیشنهاد EF بدون مدل بازسازی‌شده EF مجاز نیست.

